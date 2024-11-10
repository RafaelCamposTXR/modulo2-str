using System;

public abstract class ProtecaoBase
{
    public string? IED { get; set; }
    public float? limiar50 { get; set; }

    public float? limiar51 { get; set; }

    //private readonly GerenciaAlerta gerenciaAlerta;

    public ProtecaoBase(string? ied, float? limiar50, float? limiar51)
    {
        this.IED = ied;
        this.limiar50 = limiar50;
        this.limiar51 = limiar51;
    }


    public abstract bool verificarSobrecorrente(float correnteAtual);


    public void EmitirAlerta()
    {
        // aqui era pra ser o começo de gerar pacote broadcast:
        //var mensagem = new MensagemBroadcast(IED, limiar.Value, "Sobrecorrente Instantânea");
        //gerenciaAlerta.EnviarAlerta(mensagem);
        Console.WriteLine($"\nAlerta! Sobrecorrente detectada no IED: {IED}.");
    }
}
