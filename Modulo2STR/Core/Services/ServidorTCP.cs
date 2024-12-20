﻿using Modulo2STR.Core.utils;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Modulo2STR.Core.Services
{
    public class ServidorTcp
    {
        private readonly int _porta;

        public ServidorTcp(int porta)
        {
            _porta = porta;
        }

        public async Task IniciarAsync()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, _porta);
            listener.Start();
            ConsoleWrapper.WriteLine($"Servidor TCP escutando na porta {_porta}", "ciano");

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();

                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                string mensagemRecebida = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                ConsoleWrapper.WriteLine($"-- Alerta capturado pelo Servidor TCP: {mensagemRecebida}", "ciano");

                client.Close();
            }
        }
    }
}
