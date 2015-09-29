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
    public class PlayInfoController : Controller
    {
        //
        // GET: /PlayInfo/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PlayInfo()
        {
            PlayInfoBLL playbll = new PlayInfoBLL();
            string pageindex = Request["pi"];
            int pagesize = 3;
            int pindex = 0;
            int recordCount = 0;
            int iPageCount = 0;
            int.TryParse(pageindex, out pindex);
            if (pindex == 0)
            {
                pindex = 1;
            }
            List<PlayInfo> playlist = playbll.GetAllPlayInfoPage(pindex, pagesize, out recordCount);

            return View(playlist);
        }
        public ActionResult CreatePlay()
        {
            IDictionary<string, object> idiction = new Dictionary<string, object>();
            //绑定国家
            idiction.Add("CountryList", Global.BasedataStandard.GetAllDataArray(Global.BaseDataType.CountryList));
            //绑定种族
            idiction.Add("RaceList", Global.BasedataStandard.GetAllDataArray(Global.BaseDataType.RaceList));
            //绑定职业
            idiction.Add("JobsList", Global.BasedataStandard.GetAllDataArray(Global.BaseDataType.JobsList));
            //绑定性别
            idiction.Add("SexList", Global.BasedataStandard.GetAllDataArray(Global.BaseDataType.SexList));
            return View(idiction);
        }
        [HttpPost]
        public ActionResult CreatePlay_New()
        {
            string nickname = Request["nickname"];
            int country = int.Parse(Request["country"]);
            int race = int.Parse(Request["race"]);
            int jobs = int.Parse(Request["jobs"]);
            int sex = int.Parse(Request["sex"]);

            //var user = UserInfoBLL.CheckLogin.GetSessionInfo();
            //int uid = user != null ? user.UserId : 0;
            int addin = 0;
            //PlayInfo playinfo = new PlayInfo()
            //{
            //    NickName = nickname,
            //    Country = country,
            //    Race = race,
            //    Job = jobs,
            //    Psex = sex,
            //    Uid = uid
            //};
            //if (uid != 0)
            //{
            //    if (new PlayInfoBLL().AddPlayInfo(playinfo) > 0)
            //        addin = 1;
            //}

            return Content("{\"status\":\"" + addin + "\"}");
        }
    }
}
