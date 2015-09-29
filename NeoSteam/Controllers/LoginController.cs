using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using NeoSteam.BLL;
using NeoSteam.Model;
using BotDetect.Web.UI.Mvc;

namespace NeoSteam.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login()
        {
            string username = Request["uname"];
            string password = Request["password"];

            UserInfoBLL uinfobll = new UserInfoBLL();
            UserInfo userinfo = uinfobll.GetOneUserInfo(username,password);
            StringBuilder strBuilder = new StringBuilder();
            if (userinfo.UserName != null)
            {
                //login success
                UserInfoBLL.CheckLogin.SetSessionInfo(userinfo);
                strBuilder.Append("{\"status\":\"0\"}");
            }
            else
            {
                //login fail
                strBuilder.Append("{\"status\":\"1\"}");
            }
            return Content(strBuilder.ToString());
        }
        /// <summary>
        /// 用户退出
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginOut()
        {
            UserInfoBLL.CheckLogin.ExitSessionInfo();
            return RedirectToAction("index","home");
        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Add_Userinfo()
        {
            string username = Request["uname"];
            string password = Request["password"];
            StringBuilder strBuilder = new StringBuilder();
            UserInfoBLL userbll = new UserInfoBLL();

            if (userbll.Exist_UserInfo(username) == 0)
            {
                if (userbll.AddUserInfo(username, password) > 0)
                {
                    strBuilder.Append("{\"status\":\"0\"}");
                }
                else
                {
                    strBuilder.Append("{\"status\":\"1\"}");
                }
            }
            else
            {
                strBuilder.Append("{\"status\":\"2\"}");
            }
            return Content(strBuilder.ToString());
        }
        //[HttpPost]
        //[AllowAnonymous]
        //[CaptchaValidation("CaptchaCode", "SampleCaptcha", "Incorrect CAPTCHA code!")]
        //public ActionResult SampleAction(SampleModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        // TODO: Captcha validation failed, show error message      
        //    }
        //    else
        //    {
        //        // TODO: Captcha validation passed, proceed with protected action  
        //    }
        //}
    }
}
