using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NeoSteam.BLL;
using NeoSteam.Model;

namespace NeoSteam.Controllers
{
    public class IndexController : Controller
    {
        //
        // GET: /Index/

        public ActionResult Index()
        {
            return View();
        }
    }
}
