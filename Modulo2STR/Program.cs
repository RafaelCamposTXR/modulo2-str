using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // 1. Inicializa o Gerenciador de IEDs
        GerenciadorIED gerenciador = new GerenciadorIED();

        // 2. Cria e adiciona IEDs ao gerenciador
        IED ied1 = new IED("IED_01");
        IED ied2 = new IED("IED_02");
        IED ied3 = new IED("IED_03");

        gerenciador.AdicionarIED(ied1);
        gerenciador.AdicionarIED(ied2);
        gerenciador.AdicionarIED(ied3);

        // 3. Inicia o monitoramento de cada IED
        ied1.IniciarMonitoramento();
        ied2.IniciarMonitoramento();
        ied3.IniciarMonitoramento();

        Console.WriteLine("Monitoramento iniciado para todos os IEDs. Pressione qualquer tecla para simular alterações de corrente...");
        Console.ReadKey();

        // 4. Simula alterações de corrente para cada IED (pode ser ajustado conforme necessário)
        await SimularVariacoesDeCorrente(gerenciador);

        // 5. Encerra o monitoramento (opcional para finalizar o programa)
        Console.WriteLine("Pressione qualquer tecla para encerrar o monitoramento...");
        Consol
