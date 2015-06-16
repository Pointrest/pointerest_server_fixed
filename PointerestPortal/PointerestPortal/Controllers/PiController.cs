using Data;
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
    public class PiController : ApiController
    {
        PuntiInteresseRep _repository;

        public PiController()
        {
            _repository = new PuntiInteresseRep();
        }

        [HttpGet]
        public List<PIQuery> Get()
        {
            return _repository.GetAll().ToList();
        }
        [HttpGet]
        public PIQuery Get(int id)
        {
            return _repository.Get(id);
        }

        [HttpGet]
        [Route("api/pi/username/{username}")]
        public List<PIQuery> Get(string username)
        {
            return _repository.Get(username).ToList();
        }

        // POST: api/Pi
        [HttpPost]
        public void Post(int gestoreID , PuntoInteresse pi)
        {
            //return : “201” if succesfull, “403” denied, “500” internal server error
            //info: registra un nuovo punto di interesse per il gestore con id == {gestoreID}

        }

        // PUT: api/Pi/5
        [HttpPut]
        public void Put([FromBody]UpdatePIDataCommand updatedData)
        {
            _repository.Put(updatedData);
        }

        [HttpPut]
        [Route("api/pi/images/{id}")]
        public void Put(int id, [FromBody]UpdatePIImagesCommand updatImagesCommand)
        {
            _repository.Put(updatImagesCommand);
        }

        //// DELETE: api/Pi/5
        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
