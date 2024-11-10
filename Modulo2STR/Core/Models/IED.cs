using System;
using System.Threading;

public class IED
{
    public string Id { get; private set; }
    private float corrente;
    private readonly Protecao50 protecao50;
    private readonly Protecao51 protecao51;
    private Thread monitorThread;
    private bool monitorando = true;

    public IED(string id)
    {
        Id = id;
        protecao50 = new Protecao50(id);
        protecao51 = new Protecao51(id);
    }

    public float Corrente
    {
        get { return corrente; }
        set
        {
            if (corrente != value)
            {
                corrente = value;
                Console.WriteLine($"\n\nCorrente do IED {Id} atualizada para {corrente} A");
                VerificarProtecoes();
            }
        }
    }

    public void IniciarMonitoramento()
    {
        monitorThread = new Thread(() =>
        {
            while (monitorando)
            {
                Thread.Sleep(100); 
            }
        });
        monitorThread.Start();
    }

    public void PararMonitoramento()
    {
        monitorando = false;
        if (monitorThread != null && monitorThread.IsAlive)
        {
            monitorThread.Join();
        }
    }

    private void VerificarProtecoes()
    {
        Console.WriteLine($"IED {Id}: Verificando proteções para corrente = {corrente} A");

        if (protecao50.verificarSobrecorrente(corrente))
        {
            Console.WriteLine("Sobrecorrente detectado em Protecao 50.");
            return;
        }

        if (protecao51.verificarSobrecorrente(corrente))
        {
            Console.WriteLine("Ultrapassou o limiar da Protecao 51.");
        }
    }
}
