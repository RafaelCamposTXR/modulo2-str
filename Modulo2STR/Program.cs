using Modulo2STR.Core.Models;
using Modulo2STR.Core.Services;
using Modulo2STR.Core.utils;
using System.Threading.Tasks;
using System.Diagnostics;

class Program
{
    static async Task Main(string[] args)
    {

        Task servidorTask = Task.Run(async () =>
        {
            ServidorTcp servidor = new ServidorTcp(5000);
            await servidor.IniciarAsync();
        });

        ConsoleWrapper.WriteLine("Servidor TCP rodando em segundo plano, simulando comportamento dos módulos 6 e 3.", "ciano");


        GerenciadorIED gerenciador = new GerenciadorIED();

        Task monitorRedeTask = Task.Run(async () =>
        {
            MonitorRede monitor = new MonitorRede(gerenciador, 4000); 
            await monitor.IniciarEscutaAsync();
        });

        EnvioMensagem envioMensagem = new EnvioMensagem();

        ConsoleWrapper.WriteLine("Monitor de Rede rodando em segundo plano, aguardando por pacotes do módulo 1.", "verde");

        // Executar o teste automático inicial
        Console.WriteLine("Executando teste de variação de corrente...");
        await SimularVariacoesDeCorrente(gerenciador);
        Console.WriteLine("Teste concluído.");


        bool continuar = true;
        ConsoleWrapper.WriteLine(
                            "\nMenu de Opções:\n" +
                            "send [ied] [corrente] - Enviar mensagem simulando pacote do módulo 1\n" +
                            "send internal [ied] [corrente] - Enviar mensagem internamente\n" +
                            "list - Visualizar IEDs registrados\n" +
                            "inativ - Alternar Monitoramento de inatividade \n" +
                            "close - Encerrar monitoramento do sistema\n" +
                            "menu - Visualizar menu de opções novamente", "magenta"
                    );

        while (continuar)
        {
            string input = Console.ReadLine();
            string[] parts = input.Split(' ');
            string opcao = parts[0];

            switch (opcao)
            {
                case "send":
                    if (parts[1] == "internal")
                    {
                        if (parts.Length < 4)
                        {
                            Console.WriteLine("Comando inválido. Formato: send [ied] [corrente]");
                            break;
                        }

                        string ied = parts[2];
                        if (!float.TryParse(parts[^1], out float corrente))
                        {
                            Console.WriteLine("Corrente inválida. Deve ser um número decimal.");
                            break;
                        }
                        await EnviarMensagemManualInterna(envioMensagem, ied, corrente, gerenciador);
                        break;
                    }
                    else
                    {
                        if (parts.Length < 3)
                        {
                            Console.WriteLine("Comando inválido. Formato: send [ied] [corrente]");
                            break;
                        }

                        string ied = parts[1];
                        if (!float.TryParse(parts[^1], out float corrente))
                        {
                            Console.WriteLine("Corrente inválida. Deve ser um número decimal.");
                            break;
                        }
                        await EnviarMensagemManual(envioMensagem, ied, corrente, gerenciador);
                        break;
                    }

                case "list":
                    Console.WriteLine("Os IEDs registrados atualmente são: ");
                    Console.WriteLine(gerenciador.ObterNomesIEDsFormatados());
                    break;

                case "inativ":
                    gerenciador.monitorarInatividade = !gerenciador.monitorarInatividade;
                    break;

                case "close":
                    continuar = false;
                    break;

                case "menu":
                    ConsoleWrapper.WriteLine(
                            "\nMenu de Opções:\n" +
                            "send [ied] [corrente] - Enviar mensagem simulando pacote do módulo 1\n" +
                            "send internal [ied] [corrente] - Enviar mensagem internamente\n" +
                            "list - Visualizar IEDs registrados\n" +
                            "inativ - Alternar Monitoramento de inatividade \n" +
                            "close - Encerrar monitoramento do sistema\n" +
                            "menu - Visualizar menu de opções novamente", "magenta"
                    );
                    break;

                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }

            await Task.Delay(100);
        }

        // Fim do programa

        Console.WriteLine("Encerrando monitoramento...");
        gerenciador.PararMonitoramento();


        Console.WriteLine("Monitoramento encerrado. A aplicação pode ser encerrada.");

        await Task.WhenAll(servidorTask, monitorRedeTask);
    }

    private static async Task SimularVariacoesDeCorrente(GerenciadorIED gerenciador)
    {
        EnvioMensagem envioMensagem = new EnvioMensagem();

        // Simulação de variações de corrente enviando diferentes mensagens
        await envioMensagem.EnviarMensagemIedCorrenteAsync("127.0.0.1", 4000, "ied2", 5000);
        await Task.Delay(500);

        await envioMensagem.EnviarMensagemIedCorrenteAsync("127.0.0.1", 4000, "ied1", 20);
        await Task.Delay(500);

        await envioMensagem.EnviarMensagemIedCorrenteAsync("127.0.0.1", 4000, "ied1", 300);
        await Task.Delay(500);

        await envioMensagem.EnviarMensagemIedCorrenteAsync("127.0.0.1", 4000, "ied1", 350);
        await Task.Delay(500);

        await envioMensagem.EnviarMensagemIedCorrenteAsync("127.0.0.1", 4000, "ied2", 90);
        await Task.Delay(500);

        await envioMensagem.EnviarMensagemIedCorrenteAsync("127.0.0.1", 4000, "ied2", 489);
        await Task.Delay(500);

        await envioMensagem.EnviarMensagemIedCorrenteAsync("127.0.0.1", 4000, "ied1", 350);
        await Task.Delay(500);

        await envioMensagem.EnviarMensagemIedCorrenteAsync("127.0.0.1", 4000, "ied1", 400);
        await Task.Delay(500);

        await envioMensagem.EnviarMensagemIedCorrenteAsync("127.0.0.1", 4000, "ied1", 450);
        await Task.Delay(500);

        await envioMensagem.EnviarMensagemIedCorrenteAsync("127.0.0.1", 4000, "ied1", 490);
        await Task.Delay(500);

        await envioMensagem.EnviarMensagemIedCorrenteAsync("127.0.0.1", 4000, "ied1", 495);
        await Task.Delay(500);

        await envioMensagem.EnviarMensagemIedCorrenteAsync("127.0.0.1", 4000, "ied1", 90);
        await Task.Delay(500);
    }

    private static async Task EnviarMensagemManual(EnvioMensagem envioMensagem, String idIed, float corrente, GerenciadorIED gerenciador)
    {

        await envioMensagem.EnviarMensagemIedCorrenteAsync("127.0.0.1", 4000, idIed, corrente);
    }

    private static async Task EnviarMensagemManualInterna(EnvioMensagem envioMensagem, String idIed, float corrente, GerenciadorIED gerenciador)
    {

        Stopwatch stopwatch = Stopwatch.StartNew();
        await gerenciador.AtualizarOuCriarIED(idIed, corrente, stopwatch);
    }
}
