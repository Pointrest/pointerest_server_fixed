using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace DTO
{
    public class PIQuery
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public int CategoriaID { get; set; }
        public string Categoria { get; set; }
        public int SottocategoriaID { get; set; }
        public string Sottocategoria { get; set; }
        public string Descrizione { get; set; }
        public double Latitudine { get; set; }
        public double Longitudine { get; set; }
        public List<ImmaginePIQuery> Images { get; set; }

        public PIQuery() { }
    }
}