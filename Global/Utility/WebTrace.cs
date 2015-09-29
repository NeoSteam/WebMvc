using System;
using System.Web;
using System.Diagnostics;
using System.Collections.Generic;

namespace Global
{
    public class WebTrace
    {
        private bool _isTrace = false;
        private string _msg = string.Empty;
        private Stopwatch sw = new Stopwatch();

        public WebTrace()
        {
            if (HttpContext.Current.Request.QueryString["qebgenesis"] == "y")
                this._isTrace = true;
        }

        public void StartTrace(string m)
        {
            if (this._isTrace == true)
            {
                this._msg = m;
                HttpContext.Current.Response.Write("<br>#<font color='blue'> " + this._msg + " </font># Start Trace...<br>");
                sw.Restart();
            }

        }

        public void EndTrace()
        {
            if (this._isTrace == true)
            {
                sw.Stop();
                HttpContext.Current.Response.Write("<br>#<font color='blue'> " + this._msg + " </font># Trace End. <font color='red'>Time Elapsed: <b>" + sw.ElapsedMilliseconds.ToString("N0") + " ms</b></font><hr>");
            }
        }

        public void WriteString(string m)
        {
            if (this._isTrace == true)
            {
                HttpContext.Current.Response.Write("<br><font color='red'>##</font> " + m + " <font color='red'>##</font><hr>");
            }
        }
    }
}