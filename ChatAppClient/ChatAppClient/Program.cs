using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Client
{
    static void Main()
    {
        Console.WriteLine("Client is running...");
        TcpClient client = new TcpClient("localhost", 12346); 
        NetworkStream stream = client.GetStream();
        string? name = "Client";



        try
        {
            Console.WriteLine("Choose a name: ");
            name = Console.ReadLine();

            while (true)
            {
                Console.Write("You: ");
                string? message = Console.ReadLine();
                if(message == "stream Close")
                {
                    stream.Close();
                    client.Close();
                    break;
                }
                string readyMessage = name + "%_SPLIT_%" + message;
                byte[] buffer = Encoding.ASCII.GetBytes(readyMessage);
                stream.Write(buffer, 0, buffer.Length);
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine("IOException: " + ex.Message);
        }
        finally
        {
            client.Close();
        }
    }
}
