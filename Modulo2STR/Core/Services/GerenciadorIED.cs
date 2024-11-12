using Modulo2STR.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading;

public class GerenciadorIED
{
    private Dictionary<string, IED> iedLista = new Dictionary<string, IED>();

    public void ReceberMensagem(MensagemCorrente dados)
    {
        if (dados != null)
        {
            AtualizarOuCriarIED(dados.IED, dados.Corrente);
        }
    }

    public void AtualizarOuCriarIED(string id, float novaCorrente)
    {
        if (iedLista.ContainsKey(id))
        {
            Console.WriteLine($"IED {id} identificado na lista de IEDs existentes");
            iedLista[id].Corrente = novaCorrente;
            
        }
        else
        {
            Console.WriteLine($"IED {id} adicionado à lista de IEDs monitorados");
            var novoIed = new IED(id);
            novoIed.Corrente = novaCorrente;
            novoIed.IniciarThread();
            iedLista[id] = novoIed;
            
        }
    }

    public void VerificarInatividade()
    {
        foreach (var ied in iedLista.Values)
        {
            TimeSpan tempoInatividade = DateTime.Now - ied.UltimaAtualizacao;

            if (tempoInatividade.TotalMinutes > 10)
            {
                Console.WriteLine($"IED {ied.Id} inativo por mais de 10 minutos. Desligando monitoramento.");
                ied.DesligarThread();
            }
        }
    }

    public void IniciarVerificacaoInatividade()
    {
        var timer = new Timer((e) =>
        {
            VerificarInatividade();
        }, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
    }

    public void PararMonitoramento()
    {
        Console.WriteLine("Encerramento do monitoramento de todos os IEDs em andamento");
        foreach (var ied in iedLista.Values)
        {
            ied.DesligarThread(); 
        }

        iedLista.Clear(); 
        Console.WriteLine("O monitoramento de todos os IEDs foi encerrado.");
    }
}
