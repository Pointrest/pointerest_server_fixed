using Data;
using DTO;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PointrestServerSide.Controllers
{
    public class PiController : ApiController
    {
        PuntiInteresseRep _repository;

        public PiController()
        {
            _repository = new PuntiInteresseRep();
        }

        [HttpGet]
        public List<PuntoInteresse> Get()
        {
            return _repository.GetAll().ToList();
        }
        [HttpGet]
        public PuntoInteresse Get(int id)
        {
            return _repository.Get(id);
        }

        // POST: api/Pi
        [HttpPost]
        public void Post(int gestoreID , PuntoInteresse pi)
        {
            //return : “201” if succesfull, “403” denied, “500” internal server error
            //info: registra un nuovo punto di interesse per il gestore con id == {gestoreID}

        }

        //// PUT: api/Pi/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Pi/5
        //public void Delete(int id)
        //{
        //}
    }
}
