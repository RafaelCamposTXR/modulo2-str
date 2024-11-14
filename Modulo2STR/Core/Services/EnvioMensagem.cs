using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

        public async Task EnviarPacoteDeteccaoCurtoAsync(string ipDestino, int porta, string IED, float corrente, long time)
        {
            var pacote = new
            {
                modulo = 2,
                evento = "Curto-circuito",
                IED = IED,
                Corrente = corrente,
                tempo_processamento = $"{time}ms",
                data_hora = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
            };

            string mensagem = JsonConvert.SerializeObject(pacote);

            await EnviarMensagemTcpAsync(ipDestino, porta, mensagem);
        }

        public async Task EnviarMensagemIedCorrenteAsync(string ipDestino, int porta, string IED, float corrente)
        {
            var pacote = new
            {
                modulo = 1,
                IED = IED,
                Corrente = corrente,
                data_hora = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
            };

            string mensagem = JsonConvert.SerializeObject(pacote);

            await EnviarMensagemTcpAsync(ipDestino, porta, mensagem);
        }

        public async Task EnviarAvisoInatividadeAsync(string ipDestino, int porta, string IED)
        {
            var pacote = new
            {
                modulo = 2,
                evento = "Inatividade",
                IED = IED,
                data_hora = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
            };

            string mensagem = JsonConvert.SerializeObject(pacote);

            await EnviarMensagemTcpAsync(ipDestino, porta, mensagem);
        }
    }
}
