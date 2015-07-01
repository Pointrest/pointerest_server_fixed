using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace DTO
{
    public class PIMobileQuery
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public int CategoriaID { get; set; }
        public string Categoria { get; set; }
        public int SottocategoriaID { get; set; }
        public string Sottocategoria { get; set; }
        public string Descrizione { get; set; }
        public string Indirizzo { get; set; }
        public double Latitudine { get; set; }
        public double Longitudine { get; set; }
        public List<int> ImagesID { get; set; }

        public PIMobileQuery() { }

        public PIMobileQuery(int id, string nome, int categoryID, string category, int subCategoryID, string subCategory, string descrizione
                            , string indirizzo, double latitudine, double longitudine, List<int> imagesID)
        {
            this.ID = id;
            this.Nome = nome;
            this.CategoriaID = categoryID;
            this.Categoria = category;
            this.SottocategoriaID = subCategoryID;
            this.Sottocategoria = subCategory;
            this.Descrizione = descrizione;
            this.Indirizzo = indirizzo;
            this.Latitudine = latitudine;
            this.Longitudine = longitudine;
            this.ImagesID = imagesID;
        }
    }
}