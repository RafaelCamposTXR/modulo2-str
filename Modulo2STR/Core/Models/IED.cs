using System;
using System.Threading;

public class IED
{
    public string Id { get; private set; }
    private float corrente;
    private readonly Protecao50 protecao50;
    private readonly Protecao51 protecao51;
    private Thread? monitorThread;
    private bool monitorando = true;
    public DateTime UltimaAtualizacao { get; private set; }

    public IED(string id)
    {
        Id = id;
        protecao50 = new Protecao50(id);
        protecao51 = new Protecao51(id);
        UltimaAtualizacao = DateTime.Now;
    }

    public float Corrente
    {
        get { return corrente; }
        set
        {
            if (corrente != value)
            {
                corrente = value;
                UltimaAtualizacao = DateTime.Now;
                Console.WriteLine($"\n\nCorrente do IED {Id} atualizada para {corrente} A");
                Console.WriteLine(VerificarProtecoes());
            }
        }
    }

    public void IniciarThread()
    {
        monitorThread = new Thread(() =>
        {
            while (monitorando)
            {
                // Simulando o monitoramento da corrente e verificando proteções
                Thread.Sleep(1000); // A cada segundo, verifica as proteções (apenas simulação)
                VerificarProtecoes();
            }
        });
        monitorando = true;
        monitorThread.Start();
    }

    public void DesligarThread()
    {
        monitorando = false;
        if (monitorThread != null && monitorThread.IsAlive)
        {
            monitorThread.Join();
            Console.WriteLine($"Monitoramento do IED {Id} foi parado.");
        }
    }

    private String? VerificarProtecoes()
    {
        Console.WriteLine($"IED {Id}: Verificando proteções para corrente = {corrente} A");

        if (protecao50.verificarSobrecorrente(corrente))
        {
            // Aqui você poderia chamar um método para enviar um alerta pela rede
            return "Sobrecorrente detectada em Proteção 50.";
        }

        if (protecao51.verificarSobrecorrente(corrente))
        {
            // Aqui você poderia chamar um método para enviar um alerta pela rede
            return "Sobrecorrente detectada em Proteção 51.";
        }

        return "";
    }
}
