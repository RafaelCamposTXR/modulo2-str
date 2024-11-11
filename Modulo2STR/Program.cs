using Modulo2STR.Core.Models;
using Modulo2STR.Core.Services;
using Modulo2STR.Core.utils;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        //servidor TCP em segundo plano
        Task servidorTask = Task.Run(async () =>
        {
            ServidorTcp servidor = new ServidorTcp(5000);
            await servidor.IniciarAsync();
        });

        ConsoleWrapper.WriteLine("Servidor TCP rodando em segundo plano, simulando comportamento dos módulos 6 e 3.", "ciano");

        // Criar GerenciadorIED e iniciar monitoramento de rede
        GerenciadorIED gerenciador = new GerenciadorIED();

        Task monitorRedeTask = Task.Run(async () =>
        {
            MonitorRede monitor = new MonitorRede(gerenciador, 4000); // Porta 5000, mesma do ServidorTcp
            await monitor.IniciarEscutaAsync();
        });


        ConsoleWrapper.WriteLine("Monitor de Rede rodando em segundo plano, aguardando por pacotes do módulo 1.", "verde");

        // Executar o teste automático inicial
        Console.WriteLine("Executando teste de variação de corrente...");
        await SimularVariacoesDeCorrente(gerenciador);
        Console.WriteLine("Teste concluído.");

        // Exibir menu de opções ao usuário
        bool continuar = true;
        while (continuar)
        {
            ConsoleWrapper.WriteLine("\nMenu de Opções:", "magenta");
            ConsoleWrapper.WriteLine("1. Enviar mensagem de teste manualmente", "magenta");
            ConsoleWrapper.WriteLine("2. Fechar aplicação", "magenta");

            Console.Write("Escolha uma opção: ");
            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":

                    await EnviarMensagemManual();
                    break;

                case "2":
                    continuar = false;
                    break;

                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }
            await Task.Delay(4000);
        }

        // Fim do programa

        Console.WriteLine("Encerrando monitoramento...");
        gerenciador.PararMonitoramento();


        Console.WriteLine("Monitoramento encerrado. Pressione qualquer tecla para finalizar.");
        Console.ReadKey();

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

        await envioMensagem.EnviarMensagemIedCorrenteAsync("127.0.0.1", 4000, "ied1", 350);
        await Task.Delay(500);

        await envioMensagem.EnviarMensagemIedCorrenteAsync("127.0.0.1", 4000, "ied1", 350);
        await Task.Delay(500);

        await envioMensagem.EnviarMensagemIedCorrenteAsync("127.0.0.1", 4000, "ied1", 350);
        await Task.Delay(500);

        await envioMensagem.EnviarMensagemIedCorrenteAsync("127.0.0.1", 4000, "ied2", 90);
        await Task.Delay(500);

        await envioMensagem.EnviarMensagemIedCorrenteAsync("127.0.0.1", 4000, "ied2", 110);
        await Task.Delay(500);

        await envioMensagem.EnviarMensagemIedCorrenteAsync("127.0.0.1", 4000, "ied1", 350);
        await Task.Delay(500);

        await envioMensagem.EnviarMensagemIedCorrenteAsync("127.0.0.1", 4000, "ied1", 350);
        await Task.Delay(500);

        await envioMensagem.EnviarMensagemIedCorrenteAsync("127.0.0.1", 4000, "ied1", 350);
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

    private static async Task EnviarMensagemManual()
    {

        Console.Write("Insira o ID do IED: ");
        string idIed = Console.ReadLine();


        Console.Write("Insira o valor de corrente (float): ");
        if (float.TryParse(Console.ReadLine(), out float corrente))
        {
            EnvioMensagem envioMensagem = new EnvioMensagem();
            GerenciadorIED gerenciador = new GerenciadorIED();

            //await envioMensagem.EnviarMensagemIedCorrenteAsync("127.0.0.1", 5000, idIed, corrente);
            gerenciador.AtualizarOuCriarIED(idIed, corrente);
            //Console.WriteLine($"Mensagem enviada: IED={idIed}, Corrente={corrente}");
        }
        else
        {

            Console.WriteLine("Valor de corrente inválido. Tente novamente.");
        }
    }
}
