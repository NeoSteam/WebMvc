using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NeoSteam.Model;
using NeoSteam.BLL;
using NeoSteam.Models;

namespace NeoSteam.Controllers
{
    public class OfficerController : Controller
    {
        //
        // GET: /Officer/

        public ActionResult Index(string id)
        {
            OfficerBLL offbll = new OfficerBLL();
            string pageindex = id != null ? id : "0";// Request["id"];
            int pagesize = 10;
            int pindex = 0;
            int recordCount = 0;
            int iPageCount = 0; //总页数
            int.TryParse(pageindex, out pindex);
            if (pindex == 0)
            {
                pindex = 1;
            }
            List<Officer> officerlist = offbll.GetAllOfficerPage(pindex, pagesize, out recordCount);
            iPageCount = recordCount % pagesize == 0 ? recordCount / pagesize : recordCount / pagesize + 1;
            OfficerModels offmodel = new OfficerModels();
            offmodel.PageCount = iPageCount;
            offmodel.PageIndex = pindex;
            return View(new blogModel(officerlist, offmodel));
        }
        public class blogModel
        {
            public List<Officer> types { get; private set; }
            public OfficerModels posts { get; private set; }
            public blogModel(List<Officer> types, OfficerModels posts)
            {
                this.types = types;
                this.posts = posts;
            }
        }
    }


}
