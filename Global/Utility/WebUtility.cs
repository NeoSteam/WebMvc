using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Global
{
    public class WebUtility
    {
        public static string CreatePageList(string js_fn, int yenum, int page_size, int total_row, int current_page, out int total_page)
        {
            int s_page;  //****************显示到页面上的条码数定位10.
            int e_page;
            int jump = yenum;
            total_page = Convert.ToInt32(total_row / page_size);
            total_page = (total_row % page_size > 0) ? total_page + 1 : total_page;


            System.Text.StringBuilder returnStr = new System.Text.StringBuilder();


            if (current_page < 1 || current_page > total_page) current_page = 1;

            if (current_page < 5)
            {
                s_page = 1;
                e_page = 10;
            }
            else
            {
                s_page = current_page - 4;
                e_page = current_page + 5;
            }

            if (e_page > total_page) e_page = total_page;



            string _tr = "<span class=\"mr_10\"> " + total_row.ToString() + " 条:共" + total_page.ToString() + "页</span>";

            for (int i = s_page; i <= e_page; i++)
            {
                if (i == current_page)
                {
                    returnStr.Append("<a class=\"focus\">" + i + "</a>");
                }
                else
                {
                    returnStr.Append(@"<a href=""javascript:" + js_fn + "('" + i + @"')"">" + i + "</a>");
                }
            }

            if (current_page != 1)
                returnStr.Insert(0, @"<a href=""javascript:" + js_fn + "('" + (current_page - 1) + @"')"" class=""previous"" >上一页</a>");


            if (current_page < total_page)
                returnStr.Append(@"<a href=""javascript:" + js_fn + "('" + (current_page + 1) + @"')"" class=""next"">下一页</a>");


            if (total_page > 3)
            {
                //*******************当总页数大于三的时候,此时添加首页和尾页.(需要添加跳页)


                returnStr.Insert(0, @"<a href=""javascript:" + js_fn + "('" + 1 + @"')"" class=""next"">首页</a>");
                returnStr.Append(@"<a href=""javascript:" + js_fn + "('" + total_page + @"')"" class=""next"">尾页</a>");

                returnStr.Append(@"<input type=text id='getYenum' style='height:14px; width:40px' >");
                returnStr.Append(@"<a id ='tiaozhuan' href=""javascript:" + js_fn + "('" + jump + @"')"">跳转</a>");

            }


            if (total_page <= 1) returnStr.Clear();

            return _tr + returnStr.ToString();
        }
    }
}
