using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Offerta
    {
        public int IDOfferta { get; set; }

        public int IDpuntoInteresse { get; set; }
        public string Nome { get; set; }
        public string Descrizione { get; set; }
    }
}
