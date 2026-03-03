using System.Net;
using System.Net.Sockets;
using System.Text;

TcpListener server = new TcpListener(IPAddress.Any, 13000);
server.Start();
Console.WriteLine("--- Aguardando Cliente no Servidor ---");

using TcpClient client = server.AcceptTcpClient();
using NetworkStream stream = client.GetStream();

while (true)
{
    byte[] buffer = new byte[1024];
    int bytesRead = stream.Read(buffer, 0, buffer.Length);
    if (bytesRead == 0) break;

    string msgRecebida = Encoding.ASCII.GetString(buffer, 0, bytesRead);
    Console.WriteLine("Cliente: " + msgRecebida);

    Console.Write("Resposta: ");
    string? resposta = Console.ReadLine();
    if (resposta == null) resposta = "OK";

    byte[] msgEnviar = Encoding.ASCII.GetBytes(resposta);
    stream.Write(msgEnviar, 0, msgEnviar.Length);
}