using Modulo2STR.Core.Models;
using Modulo2STR.Core.Services;

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

        
        string idIed = "IED12345";  
        float corrente = 123.45f;  


        var mensagem = new MensagemCorrente(idIed, corrente);




        Console.WriteLine("Monitoramento iniciado para todos os IEDs. Pressione qualquer tecla para simular alterações de corrente...");
        Console.ReadKey();

        gerenciador.ReceberMensagem(mensagem);

        await SimularVariacoesDeCorrente(gerenciador);

        Console.WriteLine("Pressione qualquer tecla para encerrar o monitoramento...");
        Console.ReadKey();
        gerenciador.PararMonitoramento();

        Console.WriteLine("Monitoramento encerrado.");

        await servidorTask;
    }

    private static async Task SimularVariacoesDeCorrente(GerenciadorIED gerenciador)
    {
        gerenciador.AtualizarOuCriarIED("ied1", 5000);
        await Task.Delay(500);

    }


}
