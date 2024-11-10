class Program
{
    static async Task Main(string[] args)
    {
        GerenciadorIED gerenciador = new GerenciadorIED();

        IED ied1 = new IED("IED_01");
        IED ied2 = new IED("IED_02");
        IED ied3 = new IED("IED_03");

        gerenciador.AdicionarIED(ied1);
        gerenciador.AdicionarIED(ied2);
        gerenciador.AdicionarIED(ied3);

        gerenciador.IniciarMonitoramentoTodos();

        Console.WriteLine("Monitoramento iniciado para todos os IEDs. Pressione qualquer tecla para simular alterações de corrente...");
        Console.ReadKey();

        await SimularVariacoesDeCorrente(gerenciador);

        Console.WriteLine("Pressione qualquer tecla para encerrar o monitoramento...");
        Console.ReadKey();
        gerenciador.PararMonitoramentoTodos();

        Console.WriteLine("Monitoramento encerrado.");
    }

    private static async Task SimularVariacoesDeCorrente(GerenciadorIED gerenciador)
    {
        gerenciador.AtualizarIED("IED_01", 50f);
        await Task.Delay(500);

        gerenciador.AtualizarIED("IED_01", 498f);
        await Task.Delay(500);

        gerenciador.AtualizarIED("IED_01", 498f);
        await Task.Delay(500);

        gerenciador.AtualizarIED("IED_01", 498f);
        await Task.Delay(500);

        gerenciador.AtualizarIED("IED_01", 498f);
        await Task.Delay(500);

        gerenciador.AtualizarIED("IED_01", 498f);
        await Task.Delay(500);

        gerenciador.AtualizarIED("IED_01", 498f);
        await Task.Delay(500);

        gerenciador.AtualizarIED("IED_01", 499f);
        await Task.Delay(500);

        gerenciador.AtualizarIED("IED_01", 498f);
        await Task.Delay(500);

        gerenciador.AtualizarIED("IED_01", 498f);
        await Task.Delay(500);

        gerenciador.AtualizarIED("IED_01", 50f);
        await Task.Delay(500);

        gerenciador.AtualizarIED("IED_01", 60f);
        await Task.Delay(500);

        gerenciador.AtualizarIED("IED_01", 50f);
        await Task.Delay(500);

        gerenciador.AtualizarIED("IED_01", 50f);
        await Task.Delay(500);

        gerenciador.AtualizarIED("IED_01", 50f);
        await Task.Delay(500);
    }
}
