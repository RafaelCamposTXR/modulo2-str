using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modulo2STR.Core.Models
{
    public class MensagemCorrente
    {
        public string? Id { get; set; }    
        public float Corrente { get; set; } 


        public MensagemCorrente(string? id, float corrente)
        {

            Id = id;
            Corrente = corrente;
        }
    }

}
