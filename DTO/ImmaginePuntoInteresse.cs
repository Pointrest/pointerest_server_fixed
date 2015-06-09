using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ImmaginePuntoInteresse
    {
        public int ID { get; set; }
        public string Data { get; set; }
        public bool IsTombStone { get; set; }

        public ImmaginePuntoInteresse(int id, string data, bool isTombStone)
        {
            this.ID = id;
            this.Data = data;
            this.IsTombStone = isTombStone;
        }
    }
}
