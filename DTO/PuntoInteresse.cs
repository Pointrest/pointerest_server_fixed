using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace DTO
{
    public class PuntoInteresse
    {
        public int ID { get; set; }
        public int IDGestore { get; set; }
        public string Nome { get; set; }
        public string Categoria { get; set; }
        public string Sottocategoria { get; set; }
        public string Descrizione { get; set; }
        public double Latitudine { get; set; }
        public double Longitudine { get; set; }
        public List<ImmaginePuntoInteresse> Images { get; set; }

        public PuntoInteresse() { }

        /*public PuntoInteresse(int ID, int IDGestore, string nome, string categoria, string sottocategoria, 
            string descrizione, double latitudine, double longitudine, string tipo, List<ImmaginePuntoInteresse> images)
        {
            this.ID = ID;
            this.IDGestore = IDGestore;
            this.Nome = nome;
            this.Categoria = categoria;

            if (sottocategoria != null)
            {
                this.Sottocategoria = sottocategoria;
            }
            
            this.Descrizione = descrizione;
            this.Latitudine = latitudine;
            this.Longitudine = longitudine;
            this.Tipo = tipo;

            if (images != null)
            {
                this.Images = images;
            }
        }*/
        

    }
}