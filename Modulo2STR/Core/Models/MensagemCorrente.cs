using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modulo2STR.Core.Models
{
    public class MensagemCorrente
    {
        public string IED { get; set; }    
        public float Corrente { get; set; } 


        public MensagemCorrente(string ied, float corrente)
        {

            IED = ied;
            Corrente = corrente;
        }
    }

}
