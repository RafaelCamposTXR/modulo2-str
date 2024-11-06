using System;

public abstract class ProtecaoBase
{
    public string? IED { get; set; }
    public float limiar { get; set; }

    public ProtecaoBase(string? ied, float limiar)
    {
        this.IED = ied;
        this.limiar = limiar;
    }


    public abstract bool verificarSobrecorrente(float correnteAtual);


    public String EmitirAlerta()
    {
        return $"Alerta! Sobrecorrente detectada no IED: {IED}. Limite de {limiar}A ultrapassado.";
    }
}



