using DTO;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PointerestPortal.Controllers
{
    public class SottocategorieController : ApiController
    {
        SottocategorieRepository _repository;

        public SottocategorieController()
        {
            _repository = new SottocategorieRepository();
        }
        // GET: api/Categorie
        public IEnumerable<Sottocategoria> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        //// GET: api/Categorie/5
        public IEnumerable<Sottocategoria> Get(int id)
        {
            return _repository.Get(id);
        }

        //// POST: api/Sottocategorie
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Sottocategorie/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Sottocategorie/5
        //public void Delete(int id)
        //{
        //}
    }
}
