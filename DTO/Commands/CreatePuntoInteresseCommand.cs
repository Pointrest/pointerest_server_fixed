using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CreatePuntoInteresseCommand
    {
        public string Nome { get; set; }
        public int SottocategoriaID { get; set; }
        public string Descrizione { get; set; }
        public string Indirizzo { get; set; }
        public double Latitudine { get; set; }
        public double Longitudine { get; set; }
        public List<string> Images { get; set; }
    }
}
