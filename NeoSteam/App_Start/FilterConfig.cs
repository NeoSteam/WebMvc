using System.Web;
using System.Web.Mvc;
using NeoSteam.Controllers.Power;

namespace NeoSteam
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CheckInLoginController());
        }
    }
}