using ChatApp;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


class Server
{
    

    static TcpListener listener;
    static List<TcpClient> connectedClients = new List<TcpClient>();

    static void Main()
    {
        try
        {
            int port = 12346;

            listener = new TcpListener(IPAddress.Any, port);
            Logger.Log((int)Logger.LogType.Success, "Server is running on port " + port.ToString());
            listener.Start();

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                connectedClients.Add(client);
                Logger.Log((int)Logger.LogType.Info, "Client connected -> " + GetClientIdentifier(client));

                Thread clientThread = new (() => HandleClient(client));
                clientThread.Start();
            }

        }
        catch(Exception ex)
        {

            Logger.Log((int)Logger.LogType.Fatal, ex.Message);

        }
        

    }

    static void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        try
        {

            while (true)
            {
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);

                if(bytesRead <= 0)
                {
                    connectedClients.Remove(client);
                    Logger.Log((int)Logger.LogType.Info, "Client disconnected -> " + GetClientIdentifier(client));
                    break;
                }
                
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                string[] splittedMessage = SplitMessageHeaders(message);
                string messageHeaderName = splittedMessage[0].Trim();
                string messageHeaderValue = splittedMessage[1].Trim();

                Logger.Log((int)Logger.LogType.Message, GetClientIdentifier(client) + " " + messageHeaderName + ": " + messageHeaderValue);
                Broadcast(message);
                    
            }
        }
        catch (IOException ex) when (ex.InnerException is SocketException socketException && socketException.ErrorCode == 10054)
        {
            connectedClients.Remove(client);
            Logger.Log((int)Logger.LogType.Info, "Client disconnected -> " + GetClientIdentifier(client));
        }
        catch (Exception ex)
        {
            Logger.Log((int)Logger.LogType.Fatal, ex.Message);
        }
        finally
        {
            client.Close();
        }
    }
    
    static string GetClientIdentifier(TcpClient client)
    {

        IPEndPoint endPoint = null;
        endPoint = (IPEndPoint)client.Client.RemoteEndPoint;
        return endPoint.Address.ToString() + " : " + endPoint.Port.ToString();
    }

    static void Broadcast(string message)
    {
        foreach(var connectedClient in connectedClients)
        {
            try
            {
                NetworkStream clientStream = connectedClient.GetStream();
                byte[] buffer = Encoding.ASCII.GetBytes(message);
                clientStream.Write(buffer, 0, buffer.Length);
                clientStream.Flush();

            }catch(Exception ex)
            {
                Logger.Log((int)Logger.LogType.Fatal, ex.Message);
            }
        }
    }

    static string[] SplitMessageHeaders(string message)
    {
        string[] splittedMessage = message.Split("%_SPLIT_%");

        return splittedMessage;

    }

}
