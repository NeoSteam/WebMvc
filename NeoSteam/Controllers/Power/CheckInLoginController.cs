using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NeoSteam.Models;
using NeoSteam.Model;
using NeoSteam.BLL;

namespace NeoSteam.Controllers.Power
{
    public class CheckInLoginController : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToUpper() == "LOGIN")
            {
                return;
            }
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                UserInfo um = UserInfoBLL.CheckLogin.GetSessionInfo();
                if (um == null)
                {
                    filterContext.Result = new RedirectResult("/Login/Index");
                    return;
                }
            }
        }
    }
}
