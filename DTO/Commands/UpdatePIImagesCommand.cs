using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Commands
{
    public class UpdatePIImagesCommand
    {
        public int IDPuntoInteresse { get; set; }
        public UpdatedImage[] Images { get; set; }
        public class UpdatedImage
        {
            public int ImageID { get; set; }
            public string ImageData { get; set; }
        }
    }
}
