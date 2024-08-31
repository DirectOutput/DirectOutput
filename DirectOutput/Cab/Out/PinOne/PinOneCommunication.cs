using System;
using System.IO.Pipes;
using System.Text;
using System.Threading;

public class PinOneCommunication
{
    private NamedPipeClientStream pipeClient;
    private NamedPipeServer server = null;
    private string pipeName = "ComPortServerPipe";
    private string COMPort = "";

    public PinOneCommunication(String COMPort)
    {
         this.COMPort = COMPort;
    }

    public bool ConnectToServer()
    {
        try
        {
            pipeClient = new NamedPipeClientStream(".", pipeName, PipeDirection.InOut, PipeOptions.None);
            pipeClient.Connect(100);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool DisconnectFromServer()
    {
        Disconnect();
        if (server !=null)
        {
            server.StopServer();
            return true;
        }
        return false;
    }

    public bool CreateServer()
    {
        try
        {
            if (!COMPort.IsNullOrEmpty())
            {
                server = new NamedPipeServer(COMPort);
                Thread serverThread = new Thread(server.StartServer);
                serverThread.IsBackground = true;
                serverThread.Start();
                Thread.Sleep(300);
                return true;
            }
        }
        catch (Exception)
        {
            return false;
        }

        return false;
    }
    public bool isComPortConnected()
    {
        SendMessage("CHECK");
        var response = ReadMessage();
        return response == "TRUE";
    }
    public void Disconnect()
    {
        SendMessage("DISCONNECT");
    }

    public void Write(byte[] bytesToWrite)
    {
        string base64Bytes = Convert.ToBase64String(bytesToWrite);
        SendMessage($"WRITE {base64Bytes}");
        ReadMessage(); // Expect OK
    }

    public string ReadLine()
    {
        SendMessage("READLINE");
        return ReadMessage();
    }

    public string GetCOMPort()
    {
        SendMessage("COMPORT");
        return ReadMessage();
    }

    private void SendMessage(string message)
    {
        try
        {
            byte[] request = Encoding.UTF8.GetBytes(message);
            pipeClient.Write(request, 0, request.Length);
        }
        catch (Exception)
        {
            if (CreateServer() && ConnectToServer())
            {
                byte[] request = Encoding.UTF8.GetBytes(message);
                pipeClient.Write(request, 0, request.Length);
            }
            else
            {
                throw new Exception("Unable to connect to board");
            }

        }

    }

    private string ReadMessage()
    {
        try
        {
            var response = new byte[1024];
            int bytesRead = pipeClient.Read(response, 0, response.Length);
            return Encoding.UTF8.GetString(response, 0, bytesRead);
        }
        catch (Exception)
        {
            if( CreateServer() && ConnectToServer())
            {
                return ReadMessage();
              
            } else
            {
                return "";
            }
        }

    }
}
