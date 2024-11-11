using Modulo2STR.Core.Models;
using Modulo2STR.Core.Services;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {

        Task servidorTask = Task.Run(async () =>
        {
            ServidorTcp servidor = new ServidorTcp(5000);
            await servidor.IniciarAsync();
        });

        Console.WriteLine("Servidor TCP rodando em segundo plano.");


        GerenciadorIED gerenciador = new GerenciadorIED();


        Task monitorRedeTask = Task.Run(async () =>
        {
            MonitorRede monitor = new MonitorRede(gerenciador, 5000); // Porta 5000, mesma do ServidorTcp
            await monitor.IniciarEscutaAsync();
        });

        Console.WriteLine("Monitor de Rede rodando em segundo plano.");


        string idIed = "IED12345";
        float corrente = 123.45f;

        var mensagem = new MensagemCorrente(idIed, corrente);


        EnvioMensagem envioMensagem = new EnvioMensagem();


        Console.WriteLine("Enviando mensagem com IED e corrente...");
        await envioMensagem.EnviarMensagemIedCorrenteAsync("127.0.0.1", 5000, idIed, corrente);

        Console.WriteLine("Mensagem enviada.");

        Console.WriteLine("Monitoramento iniciado para todos os IEDs. Pressione qualquer tecla para simular alterações de corrente...");
        Console.ReadKey();


        await SimularVariacoesDeCorrente(gerenciador);

        Console.WriteLine("Pressione qualquer tecla para encerrar o monitoramento...");
        Console.ReadKey();
        gerenciador.PararMonitoramento();

        Console.WriteLine("Monitoramento encerrado.");

        await Task.WhenAll(servidorTask, monitorRedeTask);
    }

    private static async Task SimularVariacoesDeCorrente(GerenciadorIED gerenciador)
    {
        gerenciador.AtualizarOuCriarIED("ied1", 5000);
        await Task.Delay(500);
    }
}
