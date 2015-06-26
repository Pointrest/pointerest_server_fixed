using Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
        public HttpResponseMessage Get(int id)
        {
            String immgine64 =  _rep.Get(id);
            String s = immgine64.Split(',')[1];
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(new MemoryStream(Convert.FromBase64String(s)));
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = id + ".png";
            return result;
        }
    }
}
