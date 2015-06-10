using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Categoria
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }

        public Categoria() { }

        public Categoria(int ID, string CategoryName) {
            this.CategoryName = CategoryName;
            this.ID = ID;
        }
    }
}
