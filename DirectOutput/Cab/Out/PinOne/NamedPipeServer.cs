using System;
using System.IO.Ports;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class NamedPipeServer
{
    private SerialPort serialPort;
    private bool isRunning = true;
    private string comPort = "";
    private const string PipeName = "ComPortServerPipe";
    CancellationToken clientToken = new CancellationToken();
    CancellationToken serverToken = new CancellationToken();

    public NamedPipeServer(string comPort)
    {
        this.comPort = comPort;
        serialPort = new SerialPort(comPort, 2000000, Parity.None, 8, StopBits.One);
        serialPort.NewLine = "\r\n";

        // TIMEOUTS REDUCED: Prevents the 500ms freezing during lag!
        serialPort.ReadTimeout = 50;
        serialPort.WriteTimeout = 50;

        try
        {
            serialPort.Open();
            serialPort.DtrEnable = true;
        }
        catch (Exception ex)
        {
            // Soft-Fail: If the port is missing at startup, log it but don't crash
            Console.WriteLine($"[PinOne Watchdog] Initial startup warning: Could not open {comPort}. Will retry automatically. ({ex.Message})");
        }
    }

    public void StartServer()
    {
        Task.Run(async () =>
        {
            while (isRunning)
            {
                var serverStream = new NamedPipeServerStream(
                    PipeName,
                    PipeDirection.InOut,
                    NamedPipeServerStream.MaxAllowedServerInstances,
                    PipeTransmissionMode.Byte,
                    PipeOptions.Asynchronous);

                Console.WriteLine("Waiting for client connection...");
                try
                {
                    await serverStream.WaitForConnectionAsync(serverToken);
                    var awaitIgnored = HandleClientConnectionAsync(serverStream);
                }
                catch (Exception ex)
                {
                    // Safely catches cancellation exceptions during shutdown
                    if (isRunning) Console.WriteLine($"[PinOne Watchdog] Server stream error: {ex.Message}");
                }
            }
        });
    }

    private async Task HandleClientConnectionAsync(NamedPipeServerStream serverStream)
    {
        bool completed = false;
        while (isRunning && !completed && serverStream.IsConnected)
        {
            try
            {
                var request = new byte[1024];
                int bytesRead = await serverStream.ReadAsync(request, 0, request.Length, clientToken);
                if (bytesRead == 0) break; // Client disconnected cleanly

                string requestStr = Encoding.UTF8.GetString(request, 0, bytesRead);

                // Process request
                if (requestStr.StartsWith("CONNECT"))
                {
                    Console.WriteLine("Requesting Connect");
                    try
                    {
                        if (!serialPort.IsOpen)
                        {
                            serialPort.Open();
                            serialPort.DtrEnable = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[PinOne Watchdog] Connect request failed to open port: {ex.Message}");
                    }
                    serverStream.Write(Encoding.UTF8.GetBytes("OK"), 0, 2);
                }
                else if (requestStr.StartsWith("STOP_SERVER"))
                {
                    isRunning = false;
                }
                else if (requestStr.StartsWith("DISCONNECT"))
                {
                    serverStream.Disconnect();
                    completed = true;
                    Console.WriteLine("Requesting disconnect");
                }
                else if (requestStr.StartsWith("WRITE"))
                {
                    var bytesToWrite = Convert.FromBase64String(requestStr.Substring(6));

                    try
                    {
                        // Auto-Reconnect under the hood, without disturbing the client!
                        if (!serialPort.IsOpen)
                        {
                            serialPort.Open();
                            serialPort.DtrEnable = true;
                        }
                        serialPort.Write(bytesToWrite, 0, bytesToWrite.Length);
                    }
                    catch (TimeoutException ex)
                    {
                        // Soft-Fail: Ignore Windows stutters/timeouts
                        Console.WriteLine($"[PinOne Warning] Soft-Fail: USB stutter or timeout detected, frame dropped. ({ex.Message})");
                    }
                    catch (Exception ex)
                    {
                        // Hard-Fail: EMI Glitch. Close port so it opens cleanly in the next loop.
                        Console.WriteLine($"[PinOne Critical] Hard-Fail: Hardware connection dropped (EMI Glitch?). Closing port for auto-recovery. ({ex.Message})");
                        try { serialPort.Close(); } catch { }
                    }

                    // LIFESAVER: We ALWAYS report "OK" to the client. 
                    // This ensures the 300ms loop of death in the DOF thread is never triggered!
                    try { serverStream.Write(Encoding.UTF8.GetBytes("OK"), 0, 2); } catch { }
                }
                else if (requestStr.StartsWith("READLINE"))
                {
                    string response = "";
                    try
                    {
                        if (!serialPort.IsOpen)
                        {
                            serialPort.Open();
                            serialPort.DtrEnable = true;
                        }
                        response = serialPort.ReadLine();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[PinOne Warning] ReadLine failed: {ex.Message}");
                        try { serialPort.Close(); } catch { }
                    }
                    try { serverStream.Write(Encoding.UTF8.GetBytes(response), 0, response.Length); } catch { }
                }
                else if (requestStr.StartsWith("CHECK"))
                {
                    string response = serialPort.IsOpen ? "TRUE" : "FALSE";
                    try { serverStream.Write(Encoding.UTF8.GetBytes(response), 0, response.Length); } catch { }
                }
                else if (requestStr.StartsWith("COMPORT"))
                {
                    Console.WriteLine("Requesting com port");
                    try { serverStream.Write(Encoding.UTF8.GetBytes(this.comPort), 0, this.comPort.Length); } catch { }
                }
            }
            catch (Exception ex)
            {
                // If the pipe breaks, the server is NO LONGER completely terminated (isRunning = false).
                // It only disconnects this specific client and immediately waits for the next one!
                Console.WriteLine($"[PinOne Watchdog] Client pipe disconnected unexpectedly: {ex.Message}");
                serverStream.Disconnect();
                completed = true;
            }
        }

        if (serverStream.IsConnected)
        {
            serverStream.Disconnect();
        }
        serverStream.Close();
    }

    public void StopServer()
    {
        isRunning = false;
        try { serialPort.Close(); } catch { }
        try { clientToken.ThrowIfCancellationRequested(); } catch { }
        try { serverToken.ThrowIfCancellationRequested(); } catch { }
        Thread.Sleep(300);
    }
}