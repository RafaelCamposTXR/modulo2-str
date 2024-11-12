public class Protecao50 : ProtecaoBase
{
    public Protecao50(string? IED = null, float? limiar50 = null, float? limiar51 = null)
        : base(IED, limiar50 ?? 500f, limiar51 ?? 100f)  
    {
    }

    public override Task<bool> verificarSobrecorrente(float corrente)
    {
        if (corrente > limiar50)
        {
            EmitirAlerta("Proteção 50");
            return Task.FromResult(true);
        }
        else
        {
            return Task.FromResult(false);
        }
    }
}
