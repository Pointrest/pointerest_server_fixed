using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Repositories;
using DTO;

namespace PointerestPortal.Controllers
{
    public class CategorieController : ApiController
    {
        CategorieRepository _repository;

        public CategorieController()
        {
            _repository = new CategorieRepository();
        }

        // GET: api/Categorie
        public IEnumerable<Categoria> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        // GET: api/Categorie/5
        public Categoria Get(int id)
        {
            return _repository.Get(id);
        }

        //// POST: api/Categorie
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Categorie/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Categorie/5
        //public void Delete(int id)
        //{
        //}
    }
}
