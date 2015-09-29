using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NeoSteam.Model;
using NeoSteam.BLL;

namespace NeoSteam.Controllers
{
    public class UserOfficerListController : Controller
    {
        //
        // GET: /UserOfficer/

        public ActionResult Index()
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
