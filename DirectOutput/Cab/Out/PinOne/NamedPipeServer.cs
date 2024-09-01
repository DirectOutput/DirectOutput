using System;
using System.IO.Ports;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PerCederberg.Grammatica.Runtime;

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
        serialPort.ReadTimeout = 500;
        serialPort.WriteTimeout = 500;
        serialPort.Open();
        serialPort.DtrEnable = true;
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
                await serverStream.WaitForConnectionAsync(serverToken);

                var awaitIgnored = HandleClientConnectionAsync(serverStream);
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
                string requestStr = Encoding.UTF8.GetString(request, 0, bytesRead);

                // Process request
                if (requestStr.StartsWith("CONNECT"))
                {
                    Console.WriteLine("Requesting Connect");
                    serialPort.Open();
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
                    serialPort.Write(bytesToWrite, 0, bytesToWrite.Length);
                    serverStream.Write(Encoding.UTF8.GetBytes("OK"), 0, 2);
                }
                else if (requestStr.StartsWith("READLINE"))
                {
                    string response = serialPort.ReadLine();
                    serverStream.Write(Encoding.UTF8.GetBytes(response), 0, response.Length);
                }
                else if (requestStr.StartsWith("CHECK"))
                {
                    string response = serialPort.IsOpen ? "TRUE" : "FALSE";
                    serverStream.Write(Encoding.UTF8.GetBytes(response), 0, response.Length);
                }
                else if (requestStr.StartsWith("COMPORT"))
                {
                    Console.WriteLine("Requesting com port");
                    serverStream.Write(Encoding.UTF8.GetBytes(this.comPort), 0, this.comPort.Length);
                }
            }
            catch (Exception)
            {
                serverStream.Disconnect();
                isRunning = false;
            }
            finally
            {
                //Console.WriteLine("cleaning up, closing ports");
            }

        }
        if (isRunning == false)
        {

            serverStream.Disconnect();
            serverStream.Close();
            
        }
    }

    public void StopServer()
    {
        serialPort.Close();
        isRunning = false;
        clientToken.ThrowIfCancellationRequested();
        serverToken.ThrowIfCancellationRequested();
        Thread.Sleep(300);
    }
}
