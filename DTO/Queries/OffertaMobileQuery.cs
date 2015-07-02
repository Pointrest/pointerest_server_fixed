using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Queries
{
    public class OffertaMobileQuery
    {
        public int IDOfferta { get; set; }
        public int IDPuntoInteresse { get; set; }
        public string Nome { get; set; }
        public string Descrizione { get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime DataFine { get; set; }
        public string ImmagineOfferta { get; set; }

        public OffertaMobileQuery()
        {

        }
        public OffertaMobileQuery(int IDOfferta, int IDPuntoInteresse,
            string Nome, string Descrizione, DateTime DataInizio, DateTime DataFine, string ImmagineOfferta)
        {
            this.IDOfferta = IDOfferta;
            this.IDPuntoInteresse = IDPuntoInteresse;
            this.Nome = Nome;
            this.Descrizione = Descrizione;
            this.DataInizio = DataInizio;
            this.DataFine = DataFine;
            this.ImmagineOfferta = ImmagineOfferta;
        }
    }
}
