using System.Net.Sockets;
using System.Text;

// Conectar ao servidor
TcpClient client = new TcpClient("127.0.0.1", 13000);
using NetworkStream stream = client.GetStream();
Console.WriteLine("--- Chat iniciado (Cliente) ---");

while (true)
{
    Console.Write("Tu: ");
    string? mensagem = Console.ReadLine();

    if (!string.IsNullOrEmpty(mensagem))
    {
        byte[] data = Encoding.ASCII.GetBytes(mensagem);
        stream.Write(data, 0, data.Length);

        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string resposta = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        Console.WriteLine("Servidor: " + resposta);
    }
}