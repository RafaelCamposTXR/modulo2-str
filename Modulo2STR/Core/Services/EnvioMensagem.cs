using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;  // Para facilitar a criação de JSON

namespace Modulo2STR.Core.Services
{
    public class EnvioMensagem
    {
        public async Task EnviarMensagemTcpAsync(string ipDestino, int porta, string mensagem)
        {
            try
            {
                using (TcpClient client = new TcpClient(ipDestino, porta))
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] data = Encoding.UTF8.GetBytes(mensagem);
                    await stream.WriteAsync(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar mensagem TCP: {ex.Message}");
            }
        }

        public async Task EnviarMensagemUdpAsync(string ipDestino, int porta, string mensagem)
        {
            try
            {
                using (UdpClient udpClient = new UdpClient())
                {
                    byte[] data = Encoding.UTF8.GetBytes(mensagem);
                    await udpClient.SendAsync(data, data.Length, ipDestino, porta);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar mensagem UDP: {ex.Message}");
            }
        }

        // Novo método para gerar o pacote JSON e enviar
        public async Task EnviarPacoteDeteccaoCurtoAsync(string ipDestino, int porta, string IED, int corrente)
        {
            // Criando o pacote em formato JSON
            var pacote = new
            {
                modulo = 2,
                IED = IED,
                Corrente = corrente,
                data_hora = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
            };

            // Convertendo para JSON
            string mensagem = JsonConvert.SerializeObject(pacote);

            // Enviando a mensagem via TCP ou UDP
            await EnviarMensagemTcpAsync(ipDestino, porta, mensagem);  // Exemplo com TCP
            // Ou
            // await EnviarMensagemUdpAsync(ipDestino, porta, mensagem); // Para UDP
        }
    }
}
