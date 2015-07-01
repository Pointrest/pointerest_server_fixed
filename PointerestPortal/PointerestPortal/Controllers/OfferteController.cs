using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PointerestPortal.Controllers
{
    public class OfferteController : ApiController
    {
        // GET: api/Offerte
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Offerte/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Offerte
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Offerte/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Offerte/5
        public void Delete(int id)
        {
        }

        [Route("api/offerte/structure")]
        [HttpGet]
        public IEnumerable<string> GetStructure()
        {
            return new List<string>(new string[] { "IDOfferta", "IDPuntoInteresse", "Nome", "Descrizione", "Data Inizio", "Data Fine", "Immagine"});
        }
    }
}
