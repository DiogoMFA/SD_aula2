using System.Net;
using System.Net.Sockets;
using System.Text;

// Configuração do Servidor
IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 8888);
TcpListener servidor = new TcpListener(ipEndPoint);

servidor.Start();
Console.WriteLine(">>> SERVIDOR ONLINE (Porta 8888) <<<");
Console.WriteLine("Aguardando conexão de um cliente...");

try
{
    while (true)
    {
        using TcpClient cliente = servidor.AcceptTcpClient();
        Console.WriteLine("\n[CONEXÃO] Cliente conectado!");

        using NetworkStream stream = cliente.GetStream();
        byte[] buffer = new byte[1024];

        // Ciclo de conversa com este cliente
        while (true)
        {
            int bytesLidos = stream.Read(buffer, 0, buffer.Length);
            if (bytesLidos == 0) break; // Cliente desconectou

            string pedido = Encoding.UTF8.GetString(buffer, 0, bytesLidos).Trim().ToUpper();
            Console.WriteLine($"[PEDIDO] Recebi: {pedido}");

            string resposta = "";

            // --- LÓGICA DOS DESAFIOS ---
            if (pedido.StartsWith("SOMA"))
            {
                try
                {
                    // Espera "SOMA 10 20"
                    string[] partes = pedido.Split(' ');
                    int n1 = int.Parse(partes[1]);
                    int n2 = int.Parse(partes[2]);
                    resposta = "RESULTADO: " + (n1 + n2);
                }
                catch
                {
                    resposta = "ERRO: Usa o formato 'SOMA [n1] [n2]'";
                }
            }
            else if (pedido == "DATA")
            {
                resposta = "DATA ATUAL: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            }
            else if (pedido == "RANDOM")
            {
                resposta = "NUMERO ALEATORIO: " + new Random().Next(1, 1001);
            }
            else if (pedido == "SAIR")
            {
                resposta = "ADEUS!";
                byte[] adeusBytes = Encoding.UTF8.GetBytes(resposta);
                stream.Write(adeusBytes, 0, adeusBytes.Length);
                break;
            }
            else
            {
                resposta = "COMANDO INVALIDO. Tenta: SOMA, DATA, RANDOM ou SAIR.";
            }

            // Enviar resposta
            byte[] msgResposta = Encoding.UTF8.GetBytes(resposta);
            stream.Write(msgResposta, 0, msgResposta.Length);
        }
    }
}
catch (Exception e)
{
    Console.WriteLine("Erro: " + e.Message);
}
finally
{
    servidor.Stop();
}