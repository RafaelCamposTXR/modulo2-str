using Modulo2STR.Core.Services;
using Modulo2STR.Core.utils;
using System;
using System.Diagnostics;
using System.Threading;

public class IED
{
    public string Id { get; private set; }
    private float corrente;
    private readonly Protecao50 protecao50;
    private readonly Protecao51 protecao51;
    private Thread? monitorThread;
    private bool monitorando = true;
    public DateTime UltimaAtualizacao { get;set; }
    public Stopwatch stopIED { get; set; }




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
                Console.WriteLine($"Corrente do IED {Id} atualizada para {corrente} A");
                //VerificarProtecoes(stopIED);
            }
        }
    }

    public void IniciarThread()
    {
        monitorThread = new Thread(() =>
        {
            while (monitorando)
            {
                Thread.Sleep(1000); 
                //VerificarProtecoes();
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
            Console.WriteLine($"Monitoramento do IED {Id} foi desligado.");
        }
    }

    public async Task<string?> VerificarProtecoes(Stopwatch stopIED)
    {
        Console.WriteLine($"IED {Id}: Verificando condição de circuito em corrente = {corrente} A");

        var envioMensagem = new EnvioMensagem(); 

        if (await protecao50.verificarSobrecorrente(corrente))
        {
            stopIED.Stop();
            await envioMensagem.EnviarPacoteDeteccaoCurtoAsync("127.0.0.1", 5000, Id, corrente, stopIED.ElapsedMilliseconds);
            protecao51.CancelarTemporizador();

            return "Proteção 50 identificou anomalia.";
        }

        if (await protecao51.verificarSobrecorrente(corrente))
        {
            await envioMensagem.EnviarPacoteDeteccaoCurtoAsync("127.0.0.1", 5000, Id, corrente, stopIED.ElapsedMilliseconds);
            return "Proteção 51 identificou anomalia.";
        }

        return "";
    }
}
