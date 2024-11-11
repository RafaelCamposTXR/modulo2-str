using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Modulo2STR.Core.Services
{
    public class ServidorTcp
    {
        private readonly int _porta;

        public ServidorTcp(int porta)
        {
            _porta = porta;
        }

        public async Task IniciarAsync()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, _porta);
            listener.Start();
            Console.WriteLine($"Servidor TCP escutando na porta {_porta}...");

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                Console.WriteLine("Cliente conectado.");  //uma conexão nova é feita a cada pacote, analisar isso depois

                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                string mensagemRecebida = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Mensagem capturada pelo Servidor TCP: {mensagemRecebida}");

                client.Close();
            }
        }
    }
}
