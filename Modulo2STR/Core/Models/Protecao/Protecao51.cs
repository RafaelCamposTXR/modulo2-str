public class Protecao51 : ProtecaoBase
{
    private float Idisparo;  
    private float k;  
    private float a;  

    
    public Protecao51(string? IED = null, float Idisparo = 100f, float k = 1.0f, float a = 0.02f)
        : base(IED, null)  
    {
        this.Idisparo = Idisparo;
        this.k = k;
        this.a = a;
    }

    
    public void AtualizarLimiar(float corrente)
    {
        if (corrente <= Idisparo)
        {
            limiar = null;
        }
        else
        {
            limiar = k / (float)(Math.Pow(corrente / Idisparo, a) - 1);
        }
    }

    public override bool verificarSobrecorrente(float corrente)
    {
        AtualizarLimiar(corrente);

        if (limiar == null)
        {
            return false;
        }

        return corrente > limiar;
    }
}
