using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NeoSteam.BLL;
using NeoSteam.Models;
using NeoSteam.Model;

namespace NeoSteam.Controllers
{
    /// <summary>
    /// 控制器三个职责：
    /// 1、处理跟用户的交互
    /// 2、处理业务逻辑的调用
    /// 3、指定具体的视图显示数据，并且把数据传递给视图
    /// 
    /// 控制器约定：
    /// 1、必须是非静态类
    /// 2、必须实现IController接口
    /// 3、必须是以Controller结尾命名
    /// </summary>
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            //访问任何的当前请求的数据，已经干涉响应的内容

            //Request.Cookies
            //Session
            //HttpContext.Application
            //Session["username"] = "sss";
            //if (Session["username"] != null)
            //{
            for (int i = 0; i < 2; i++)
            {
                var a = User_ExperienceBLL.class1.instance();
                List<User_Experience> list = a.userlist;
            }
                return View();
            //}
            //else
            //{
            //    return RedirectToAction("Login", "Home");
            //}
        }

        //输出Add页面
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult About()
        {
            return Content("ok");
            //ContentResult
        }
        //输出Add页面
        public ActionResult Add()
        {
            return View();
            //ContentResult
        }

        public ActionResult Empty()
        {
            return new ContentResult();
        }
        //登录验证
        [HttpPost]
        public ActionResult LoginCheck()
        {
            string username = Request["UserName"];
            string password = Request["password"];
            string lgcheck = String.Empty;

            UserInfo userinfo = new UserInfoBLL().GetOneUserInfo(username,password);
            if (!String.IsNullOrEmpty(userinfo.UserName))
            {
                Session["username"] = userinfo.UserName;
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }
        #region 角色注册
        [HttpPost]
        public ActionResult ProcessAdd(PlayInfoModels collection)
        {
            //拿到表单里面传递来的数据
            //string userName = Request["UserName"];

            //int Age = int.Parse(collection["Age"] ?? "0");
            //往数据库插入数据

            PlayInfoBLL pbll = new PlayInfoBLL();
            PlayInfo playin = new PlayInfo();
            playin.NickName = collection.NickName;

            if (pbll.AddPlayInfo(playin) > 0)
            {

            }
            else
            {

            }
            return Content("ok");
            //页面跳转到当前控制器的Index
            //return Content("成功拉");
        }
        public ActionResult BtoCInterface()
        {
            HomeBLL hbll = new HomeBLL();
            string result = hbll.BtbAddOrder();
            return Content(result);
        }
        #endregion
    }
}
