using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ImmaginePIQuery
    {
        public int ImageID { get; set; }
        public string ImageData { get; set; }
        public ImmaginePIQuery(int id, string data)
        {
            this.ImageID = id;
            this.ImageData = data;
        }
    }
}
