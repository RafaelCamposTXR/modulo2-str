using System;
using System.Collections.Generic;

public class GerenciadorIED
{
    private Dictionary<string, IED> iedLista = new Dictionary<string, IED>();

    public void AdicionarIED(IED novoIed)
    {
        if (!iedLista.ContainsKey(novoIed.Id))
        {
            iedLista[novoIed.Id] = novoIed;
            Console.WriteLine($"IED {novoIed.Id} adicionado.");
        }
        else
        {
            Console.WriteLine($"IED {novoIed.Id} já existe.");
        }
    }

    public void AtualizarIED(string dispositivoId, float novaCorrente)
    {
        if (iedLista.ContainsKey(dispositivoId))
        {
            iedLista[dispositivoId].Corrente = novaCorrente;
        }
        else
        {
            Console.WriteLine($"IED {dispositivoId} não encontrado.");
        }
    }

    public void IniciarMonitoramentoTodos()
    {
        foreach (var ied in iedLista.Values)
        {
            ied.IniciarMonitoramento();
            Console.WriteLine($"Monitoramento iniciado para o IED {ied.Id}");
        }
    }

    public void PararMonitoramentoTodos()
    {
        foreach (var ied in iedLista.Values)
        {
            ied.PararMonitoramento();
            Console.WriteLine($"Monitoramento parado para o IED {ied.Id}");
        }
    }
}
