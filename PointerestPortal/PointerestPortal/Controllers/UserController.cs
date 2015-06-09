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
    public class UserController : ApiController
    {
        private UserRepository userRespository;

        public UserController()
        {
            userRespository = new UserRepository();
        }

        //// GET: api/User
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/User/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/User
        [HttpGet]
        public HttpResponseMessage Post( User user )
        {
            if(user == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            return userRespository.Post(user);
        }

        //POST: api/User/5
        [HttpPost]
        public HttpResponseMessage Post(int id, [FromBody]int idPuntoInteresse)
        {
            if (id == 0 || idPuntoInteresse == 0)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            return userRespository.Post(id, idPuntoInteresse);
        }

        //// PUT: api/User/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/User/5
        //public void Delete(int id)
        //{
        //}
    }
}
