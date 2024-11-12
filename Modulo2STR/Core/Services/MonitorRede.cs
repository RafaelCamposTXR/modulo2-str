using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modulo2STR.Core.Services
{
    using Modulo2STR.Core.Models;
    using Modulo2STR.Core.utils;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Diagnostics;

    public class MonitorRede
    {
        private readonly GerenciadorIED gerenciadorIED;
        private readonly int porta;

        public MonitorRede(GerenciadorIED gerenciador, int porta)
        {
            this.gerenciadorIED = gerenciador;
            this.porta = porta;
        }

        public async Task IniciarEscutaAsync()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, porta);
            listener.Start();

            ConsoleWrapper.WriteLine($"Monitor escutando na porta {porta}", "verde");

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                _ = Task.Run(() => ProcessarClienteAsync(client));
            }
        }

        private async Task ProcessarClienteAsync(TcpClient client)
        {
            try
            {
                using (var stream = client.GetStream())
                {
                    byte[] buffer = new byte[1024];
                    int bytesLidos = await stream.ReadAsync(buffer, 0, buffer.Length);
                    string mensagemJson = Encoding.UTF8.GetString(buffer, 0, bytesLidos);

                    var mensagemCorrente = JsonSerializer.Deserialize<MensagemCorrente>(mensagemJson);

                    if (mensagemCorrente != null)
                    {
                        ConsoleWrapper.WriteLine("\n\n-- Mensagem Capturada pelo monitor, referente ao IED: (" +  mensagemCorrente.IED + ") e valor de corrente: " + mensagemCorrente.Corrente, "verde");
                        Stopwatch stopwatch = Stopwatch.StartNew();
                        await gerenciadorIED.ReceberMensagem(mensagemCorrente, stopwatch);
                    }
                }
            }
            catch (Exception ex)
            {
                ConsoleWrapper.WriteLine($"Erro ao processar cliente: {ex.Message}", "verde");
            }
            finally
            {
                client.Close();
            }
        }
    }

}