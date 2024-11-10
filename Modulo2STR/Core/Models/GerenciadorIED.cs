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
            AtualizarOuCriarIED(dados.Id, dados.Corrente);
        }
    }

    private void AtualizarOuCriarIED(string id, float novaCorrente)
    {
        if (iedLista.ContainsKey(id))
        {
            iedLista[id].Corrente = novaCorrente;
            Console.WriteLine($"Corrente atualizada para o IED {id}");
        }
        else
        {
            var novoIed = new IED(id);
            novoIed.Corrente = novaCorrente;
            novoIed.IniciarThread();
            iedLista[id] = novoIed;
            Console.WriteLine($"Novo IED {id} criado e monitoramento iniciado.");
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
}

public class MensagemCorrente
{
    public string Id { get; set; }
    public float Corrente { get; set; }
}
