using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UpdatePuntoInteresseCommand
    {
        public int ID { get; set; }
        public int IDGestore { get; set; }
        public string Nome { get; set; }
        public string Sottocategoria { get; set; }
        public string Descrizione { get; set; }
        public double Latitudine { get; set; }
        public double Longitudine { get; set; }
        public string Tipo { get; set; }
        public List<UpdatedImage> Images { get; set; }

        //public List<string> Images { get; set; }

        public class UpdatedImage {
            public int ImageID { get; set; }
            public string ImageData { get; set; }
        }
    }
}
