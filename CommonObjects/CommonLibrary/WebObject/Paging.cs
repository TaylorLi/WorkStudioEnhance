/* 
 * 分页辅助类
 *  
 * 创建：杜吉利   
 * 时间：2011年9月5日
 * 
 * 修改：[姓名]         
 * 时间：[时间]             
 * 说明：[说明]
 * 
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary.WebObject
{
    public class Paging
    {
        public static string GetPagingString(int buttonCount, int pageIndex, int pageSize, int recordCount, string pageUrl, string pageJS)
        {
            if (pageSize <= 0) return string.Empty;
            if (pageIndex <= 0) pageIndex = 1;
            if (recordCount == 0 || recordCount <= pageSize) return string.Empty;
            decimal page_size = Utility.NumberHelper.Rounding(((decimal)recordCount / (decimal)pageSize), Utility.NumberHelper.RoundingTypes.Ceiling, 0);
            if (page_size <= 1) return "";
            string main_footer = "<div class=\"paging\">{0}</div>";
            string disabled_previous = string.Concat("<span class=\"disabled\">", Resources.Paging.Prev, " </span>");
            string disabled_next = string.Concat("<span class=\"disabled\">", Resources.Paging.Next, " </span>");
            string current = string.Format("<span class=\"current\">{0}</span>", pageIndex);
            string link = string.Empty;
            string page_link = string.Empty;
            if (pageJS != string.Empty)
            {
                link = "<a class=\"hand\" onclick=\"" + pageJS + "\">{1}</a>";
                page_link = "<a class=\"hand\" onclick=\"" + pageJS + "\">{0}</a>";
            }
            else
            {
                string flag = "?";
                if (pageUrl.IndexOf("?") > 0) flag = "&";
                link = "<a class=\"hand\" href=\"" + pageUrl + flag + "page={0}\">{1}</a>";
                page_link = "<a class=\"hand\" href='" + pageUrl + flag + "page={0}'>{0}</a>";
            }
            string ret = "";
            if (pageIndex > buttonCount)
            {
                ret += string.Format(link, 1, Resources.Paging.First);
            }
            if (pageIndex == 1)
            {
                ret += disabled_previous;
            }
            else
            {
                ret += string.Format(link, (pageIndex - 1), Resources.Paging.Prev);
            }


            int start = pageIndex - buttonCount;
            if (start <= 0) start = 1;
            for (int i = start; i <= page_size; i++)
            {
                if (i <= ((pageIndex < buttonCount ? buttonCount : pageIndex) + buttonCount) || (i == 1 && i < 6))
                {
                    if (i == pageIndex)
                    {
                        ret += string.Format(current, i);
                    }
                    else
                    {
                        ret += string.Format(page_link, i.ToString());
                    }
                }
            }
            if (pageIndex == page_size)
            {
                ret += disabled_next;
            }
            else
            {
                ret += string.Format(link, (pageIndex + 1), Resources.Paging.Next);
            }
            if (pageIndex != page_size && recordCount / pageSize > buttonCount * 2)
            {
                ret += string.Format(link, (page_size), Resources.Paging.Last);
            }

            return string.Format(main_footer, ret);
        }
    }
}

/* css
div.paging
{
	float: left;
	padding: 3px;
	margin: 0px;
}
div.paging a
{
	border-right: #184785 1px solid;
	padding-right: 5px;
	border-top: #184785 1px solid;
	padding-left: 5px;
	padding-bottom: 2px;
	margin: 2px;
	border-left: #184785 1px solid;
	color: #184785;
	padding-top: 2px;
	border-bottom: #184785 1px solid;
	text-decoration: none;
}
div.paging a:hover
{
	border-right: #184785 1px solid;
	border-top: #184785 1px solid;
	border-left: #184785 1px solid;
	color: #184785;
	border-bottom: #184785 1px solid;
	background-color: #BFCAE6;
}
div.paging a:active
{
	border-right: #FF9A40 1px solid;
	border-top: #FF9A40 1px solid;
	border-left: #FF9A40 1px solid;
	color: #000;
	border-bottom: #FF9A40 1px solid;
}
div.paging span.current
{
	border-right: #184785 1px solid;
	padding-right: 5px;
	border-top: #184785 1px solid;
	padding-left: 5px;
	font-weight: bold;
	padding-bottom: 2px;
	margin: 2px;
	border-left: #184785 1px solid;
	color: #fff;
	padding-top: 2px;
	border-bottom: #184785 1px solid;
	background-color: #184785;
}
div.paging span.disabled
{
	border-right: #BFCAE6 1px solid;
	padding-right: 5px;
	border-top: #BFCAE6 1px solid;
	padding-left: 5px;
	padding-bottom: 2px;
	margin: 2px;
	border-left: #BFCAE6 1px solid;
	color: #BFCAE6;
	padding-top: 2px;
	border-bottom: #BFCAE6 1px solid;
}
*/