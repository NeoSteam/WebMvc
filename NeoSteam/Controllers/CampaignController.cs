using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NeoSteam.BLL;
using NeoSteam.Model;

namespace NeoSteam.Controllers
{
    public class CampaignController : Controller
    {
        //
        // GET: /Campaign/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Battle()
        {
            UserOfficerBLL user_off_bll = new UserOfficerBLL();

            UserOfficer user_officer = new Model.UserOfficer();
            //战斗力
            int power;

            user_officer.UserOfficerList = user_off_bll.GetUserOfficerList(2, out power);

            user_officer.combat_power = power;

            return View(user_officer);
        }
    }
}
