using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Commands
{
    public class CreateOffertaCommand
    {
        public int IDPuntoInteresse { get; set; }
        public string Nome { get; set; }
        public string Descrizione { get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime DataFine { get; set; }
        public string Immagine { get; set; }
    }
}
