using DTO;
using DTO.Commands;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PointrestServerSide.Controllers
{
    public class GestoreController : ApiController
    {
        private GestoreRepository gestoreRepository;

        public GestoreController()
        {
            gestoreRepository = new GestoreRepository();
        }
        //// GET: api/Gestore
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/Gestore/5
        [HttpGet]
        public Gestore Get(int id)
        {
            if (id == 0)
                return null;

            return gestoreRepository.Get(id);
        }

        // POST: api/Gestore
        [HttpPost]
        public HttpResponseMessage Post(CreateGestoreCommand gestore)
        {
            if (gestore == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            return gestoreRepository.Post(gestore);
        }

        //// PUT: api/Gestore/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE: api/Gestore/5
        public HttpResponseMessage Delete(int id)
        {
            if(id == 0)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            return gestoreRepository.Delete(id);
        }
    }
}
