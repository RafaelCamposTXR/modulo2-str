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
            Console.WriteLine($"Corrente do IED {dispositivoId} atualizada para {novaCorrente} A");
        }
        else
        {
            Console.WriteLine($"IED {dispositivoId} não encontrado.");
        }
    }
}
