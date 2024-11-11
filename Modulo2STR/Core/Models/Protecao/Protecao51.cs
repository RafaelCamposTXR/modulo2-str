using System;
using System.Threading;
using System.Threading.Tasks;

public class Protecao51 : ProtecaoBase
{
    private const float K = 80.0f;
    private const float Alpha = 2.0f;
    private CancellationTokenSource cancellationTokenSource;
    private DateTime inicioTemporizador;
    private float tempoAtrasoAtual;

    public Protecao51(string? IED = null, float? limiar50 = null, float? limiar51 = null)
        : base(IED, limiar50 ?? 500f, limiar51 ?? 100f)
    {
    }

    public override bool verificarSobrecorrente(float correnteAtual)
    {
        if (correnteAtual > limiar51)
        {
            Console.WriteLine("Fazendo os cálculos de temporização.");

            tempoAtrasoAtual = CalcularTempoAtraso(correnteAtual);
            Console.WriteLine($"tempo atraso atual: {tempoAtrasoAtual}");

            if (cancellationTokenSource == null)
            {
                IniciarTemporizador(correnteAtual);
            }

            return true;
        }
        else
        {
            CancelarTemporizador();
            return false;
        }
    }

    private float CalcularTempoAtraso(float correnteAtual)
    {
        return K / (float)(Math.Pow(correnteAtual / limiar51.Value, Alpha) - 1);
    }

    private void IniciarTemporizador(float correnteAtual)
    {
        Console.WriteLine("\nTemporizador iniciado.");
        cancellationTokenSource = new CancellationTokenSource();
        CancellationToken token = cancellationTokenSource.Token;
        inicioTemporizador = DateTime.Now;

        _ = Task.Run(async () =>
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    float tempoDecorrido = (float)(DateTime.Now - inicioTemporizador).TotalSeconds;
                    //Console.WriteLine($"tempo decorrido: {tempoDecorrido}");

                    if (tempoDecorrido >= tempoAtrasoAtual)
                    {
                        //EmitirAlerta();
                        break;
                    }

                    await Task.Delay(50, token);
                }
            }
            catch (TaskCanceledException)
            {
                
            }
        }, token);
    }

    public void CancelarTemporizador()
    {
        cancellationTokenSource?.Cancel();
        cancellationTokenSource = null;
    }
}
