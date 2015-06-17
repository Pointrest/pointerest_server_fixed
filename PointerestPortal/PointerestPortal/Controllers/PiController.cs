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

        [HttpGet]
        [Route("api/pi/filter/{latitudine}/{longitudine}/{raggio}")]
        public List<PIMobileQuery> Get(double latitudine, double longitudine, int raggio)
        {
            return _repository.GetPIInRadius(latitudine, longitudine, raggio).ToList();
        }

        // POST: api/Pi
        [HttpPost]
        [Authorize]
        [Route("api/pi/{gestoreusername}")]
        public void Post(string gestoreusername, CreatePuntoInteresseCommand createCommand)
        {
            _repository.Post(gestoreusername, createCommand);
        }

        // PUT: api/Pi/5
        [HttpPut]
        [Authorize]
        public void Put([FromBody]UpdatePIDataCommand updatedData)
        {
            _repository.Put(updatedData);
        }

        [HttpPut]
        [Authorize]
        [Route("api/pi/images/{id}")]
        public void Put(int id, [FromBody]UpdatePIImagesCommand updatImagesCommand)
        {
            _repository.Put(updatImagesCommand);
        }

        //// DELETE: api/Pi/5
        [HttpDelete]
        [Authorize]
        [Route("api/pi/{id}")]
        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
