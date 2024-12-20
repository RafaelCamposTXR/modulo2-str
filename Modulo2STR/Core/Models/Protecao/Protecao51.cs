using Modulo2STR.Core.utils;
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

    public override async Task<bool> verificarSobrecorrente(float correnteAtual)
    {
        if (correnteAtual > limiar51)
        {
            ConsoleWrapper.WriteLine($"\n({IED}) Proteção 51: Calculando temporização.", "vermelho");

            tempoAtrasoAtual = CalcularTempoAtraso(correnteAtual);
            ConsoleWrapper.WriteLine($"({IED}) Proteção 51: Tempo de atraso atual: {tempoAtrasoAtual}s", "vermelho");

            if (cancellationTokenSource == null)
            {
                return await IniciarTemporizador(correnteAtual);
            }

            return false;
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

    private async Task<bool> IniciarTemporizador(float correnteAtual)
    {
        ConsoleWrapper.WriteLine($"\n({IED}) Proteção 51: Temporizador iniciado.", "vermelho");
        cancellationTokenSource = new CancellationTokenSource();
        CancellationToken token = cancellationTokenSource.Token;
        inicioTemporizador = DateTime.Now;

        try
        {
            while (!token.IsCancellationRequested)
            {
                float tempoDecorrido = (float)(DateTime.Now - inicioTemporizador).TotalSeconds;
                //ConsoleWrapper.WriteLine($"tempo decorrido: {tempoDecorrido}", "vermelho");

                if (tempoDecorrido >= tempoAtrasoAtual)
                {
                    ConsoleWrapper.WriteLine($"({IED}) Proteção 51: Tempo decorrido pela função de temporização: {(int)(tempoDecorrido *1000)}ms", "vermelho");
                    EmitirAlerta("Proteção 51");
                    return true;
                }

                await Task.Delay(5, token);
            }
        }
        catch (TaskCanceledException)
        {
            return false;
        }

        return false;
    }

    public void CancelarTemporizador()
    {
        cancellationTokenSource?.Cancel();
        cancellationTokenSource = null;
    }
}
