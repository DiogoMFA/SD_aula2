using System.Net.Sockets;
using System.Text;

try
{
    using TcpClient cliente = new TcpClient();
    Console.WriteLine("A tentar ligar ao servidor...");
    cliente.Connect("127.0.0.1", 8888);
    Console.WriteLine("CONECTADO!");

    using NetworkStream stream = cliente.GetStream();

    while (true)
    {
        Console.WriteLine("\n------------------------------------");
        Console.WriteLine("Comandos: SOMA [n1] [n2] | DATA | RANDOM | SAIR");
        Console.Write("Escreve o teu pedido: ");
        string mensagem = Console.ReadLine();

        if (string.IsNullOrEmpty(mensagem)) continue;

        // Enviar pedido
        byte[] dadosEnvio = Encoding.UTF8.GetBytes(mensagem);
        stream.Write(dadosEnvio, 0, dadosEnvio.Length);

        if (mensagem.ToUpper() == "SAIR") break;

        // Receber resposta
        byte[] buffer = new byte[1024];
        int bytesLidos = stream.Read(buffer, 0, buffer.Length);
        string resposta = Encoding.UTF8.GetString(buffer, 0, bytesLidos);

        Console.WriteLine("RESPOSTA DO SERVIDOR: " + resposta);
     
        // Adicionamos "\n" no final para que o cliente salte uma linha
        byte[] msgResposta = Encoding.UTF8.GetBytes(resposta + "\n");
        stream.Write(msgResposta, 0, msgResposta.Length);
    }
}
catch (Exception e)
{
    Console.WriteLine("Erro no Cliente: " + e.Message);
}

Console.WriteLine("\nPrograma terminado. Prime qualquer tecla para sair...");
Console.ReadKey();