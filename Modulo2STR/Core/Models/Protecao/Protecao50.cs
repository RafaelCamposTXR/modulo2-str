public class Protecao50 : ProtecaoBase
{
    public Protecao50(string? IED = null, float? limiar = null)
        : base(IED, limiar ?? 100f)  
    {
    }

    public override bool verificarSobrecorrente(float corrente)
    {
        return corrente > limiar;
    }
}