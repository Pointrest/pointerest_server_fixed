using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PointerestPortal.Controllers
{
    public class ManageController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult AddOfferte()
        {
            return View();
        }
        [Authorize]
        public ActionResult ListOfferte()
        {
            return View();
        }
    }
}