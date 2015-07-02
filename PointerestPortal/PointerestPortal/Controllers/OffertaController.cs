using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Repositories;
using DTO.Commands;
using DTO.Queries;

namespace PointerestPortal.Controllers
{
    public class OffertaController : ApiController
    {

        OffertaRepository _repository;


        OffertaController()
        {
            _repository = new OffertaRepository();
        }
        // GET: api/Offerta
        public List<OffertaQuery> GetAll()
        {
            return _repository.GetAll().ToList();

        }

        // GET: api/Offerta/5
        public OffertaQuery Get(int id)
        {
            return _repository.Get(id);
        }

        public List<OffertaQuery> Get(string username)
        {
            return _repository.Get(username).ToList();
        }

        // POST: api/Offerta
        public void Post(string gestoreusername, CreateOffertaCommand createCommand)
        {
            
        }

        // PUT: api/Offerta/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Offerta/5
        public void Delete(int id)
        {
        }
    }
}
