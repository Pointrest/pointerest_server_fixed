using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PointerestPortal.Controllers
{
    public class ImmaginiController : ApiController
    {
        ImagesRepository _rep;
        public ImmaginiController() {

            _rep = new ImagesRepository();
        }

        // GET: api/Immagini/5
        public string Get(int id)
        {
            return _rep.Get(id);
        }
    }
}
