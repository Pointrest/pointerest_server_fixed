using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Sottocategoria
    {
        public int ID { get; set; }
        public string SubCategoryName { get; set; }
        public int CategoriaID { get; set; }

        public Sottocategoria() { }
        public Sottocategoria(int ID, string SubCategoryName, int CategoriaID) {
            this.ID = ID;
            this.SubCategoryName = SubCategoryName;
            this.CategoriaID = CategoriaID;
        }
    }
}
