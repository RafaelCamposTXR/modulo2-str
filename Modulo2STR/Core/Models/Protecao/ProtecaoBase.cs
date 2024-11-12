using Modulo2STR.Core.utils;
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


    public abstract Task<bool> verificarSobrecorrente(float correnteAtual);


    public void EmitirAlerta(string Protecao)
    {
        ConsoleWrapper.WriteLine($"\nAnomalia em {Protecao}: Sobrecorrente detectada no IED: {IED}.", "vermelho");
    }
}
