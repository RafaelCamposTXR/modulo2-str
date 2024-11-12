using Modulo2STR.Core.Models;
using Modulo2STR.Core.utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public class GerenciadorIED
{
    private Dictionary<string, IED> iedLista = new Dictionary<string, IED>();
    public bool monitorarInatividade = true;
    private readonly TimeSpan intervaloVerificacao = TimeSpan.FromSeconds(30);
    private readonly TimeSpan tempoAvisoInatividade = TimeSpan.FromMinutes(1);
    private readonly TimeSpan tempoDesligarInatividade = TimeSpan.FromMinutes(2);

    public GerenciadorIED()
    {
        IniciarThreadMonitoramentoInatividade();
    }

    public async Task ReceberMensagem(MensagemCorrente dados, Stopwatch stopwatch)
    {
        if (dados != null)
        {
            await AtualizarOuCriarIED(dados.IED, dados.Corrente, stopwatch);
        }
    }


    public async Task AtualizarOuCriarIED(string id, float novaCorrente, Stopwatch stopwatch)
    {
        if (iedLista.ContainsKey(id))
        {
            ConsoleWrapper.WriteLine($"IED ({id}) identificado na lista de IEDs existentes", "");
            iedLista[id].Corrente = novaCorrente;
            iedLista[id].UltimaAtualizacao = DateTime.Now;
            iedLista[id].stopIED = stopwatch;
            await iedLista[id].VerificarProtecoes(stopwatch);
        }
        else
        {
            ConsoleWrapper.WriteLine($"IED ({id}) adicionado à lista de IEDs monitorados", "");
            var novoIed = new IED(id)
            {
                Corrente = novaCorrente,
                UltimaAtualizacao = DateTime.Now
            };
            novoIed.IniciarThread();
            iedLista[id] = novoIed;
            await iedLista[id].VerificarProtecoes(stopwatch);
        }
    }


    private void IniciarThreadMonitoramentoInatividade()
    {
        new Thread(() =>
        {
            while (monitorarInatividade)
            {
                VerificarInatividade();
                Thread.Sleep(intervaloVerificacao);
            }
        }).Start();
    }

    public void VerificarInatividade()
    {
        var iedsParaRemover = new List<string>();

        foreach (var ied in iedLista.Values)
        {
            TimeSpan tempoInatividade = DateTime.Now - ied.UltimaAtualizacao;

            if (tempoInatividade > tempoAvisoInatividade && tempoInatividade < tempoDesligarInatividade)
            {
                ConsoleWrapper.WriteLine($"[AVISO] IED ({ied.Id}) inativo por mais de 1 minuto.", "amarelo");
            }
            else if (tempoInatividade >= tempoDesligarInatividade)
            {
                ConsoleWrapper.WriteLine($"[DESLIGAR] IED ({ied.Id}) inativo por mais de 3 minutos. Desligando e removendo da lista.", "amarelo");
                ied.DesligarThread();
                iedsParaRemover.Add(ied.Id);
            }
        }

        foreach (var id in iedsParaRemover)
        {
            iedLista.Remove(id);
            ConsoleWrapper.WriteLine($"IED ({id}) foi removido da lista de IEDs monitorados.", "amarelo");
        }
    }

    public void PararMonitoramento()
    {
        monitorarInatividade = false;
        ConsoleWrapper.WriteLine("Encerramento do monitoramento de todos os IEDs em andamento", "");
        foreach (var ied in iedLista.Values)
        {
            ied.DesligarThread();
        }

        iedLista.Clear();
        ConsoleWrapper.WriteLine("O monitoramento de todos os IEDs foi encerrado.", "");
    }

    public string ObterNomesIEDsFormatados()
    {
        var nomesIeds = new List<string>();

        foreach (var ied in iedLista.Values)
        {
            nomesIeds.Add($"({ied.Id}) :  {ied.UltimaAtualizacao}");
        }

        return string.Join("\n", nomesIeds);
    }
}
