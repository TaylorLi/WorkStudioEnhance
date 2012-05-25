/* 
 * 页面基础类
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
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Collections.Generic;

namespace CommonLibrary.WebObject
{
    public class TemplatePage : System.Web.UI.Page
    {
        public static CultureInfo defaultCulture = new CultureInfo("en-us");

        protected override void InitializeCulture()
        {

        }

        public string GetCurrentUrl()
        {
            return WebHelper.GetCurrentUrl();
        }

        public bool IsFireFox()
        {
            return WebHelper.IsFireFox(this.Page);
        }
        public void WriteXml(string xml)
        {
            WebHelper.WriteXml(this.Page, xml);
        }

        public void ResponseNoRight()
        {
            Response.Write("您无权访问该页面。");
            Response.End();
        }

        public void ResponseTimeout()
        {
            Response.Write("Timeout");
            Response.End();
        }

        public void CheckRight(bool allowed)
        {
            if (!allowed) ResponseNoRight();
        }

        public Control GetControl(string id)
        {
            return ControlHelper.GetControl(this.Page.Controls, null, id);
        }

        public T GetControl<T>(string id) where T : class, new()
        {
            return ControlHelper.GetControl<T>(Page.Controls, id);
        }

        public static void SetValues<T>(T o, string prefix)
            where T : class, new()
        {
            WebHelper.SetValues(o, prefix);
        }

        public static List<T> GetListFromRequest<T>(string prefix)
            where T : class, new()
        {
            return WebHelper.GetListFromRequest<T>(prefix);
        }

        public static List<T> GetListFromRequestForVal<T>(string prefix)
        {
            return WebHelper.GetListFromRequestForVal<T>(prefix);
        }

        public static string FormatDateToString(DateTime date)
        {
            return CommonLibrary.Utility.DateHelper.FormatDateToString(date, CommonLibrary.Utility.DateHelper.DateFormat.yyyyMMddd);
        }

        public static DateTime ConvertDate(string date)
        {
            return CommonLibrary.Utility.DateHelper.ConvertDate(date, CommonLibrary.Utility.DateHelper.DateFormat.yyyyMMddd);
        }

        #region Json Result

        public void JsonResult(bool success, string message, object extraData)
        {
            Response.Write(JsonHelper.GetJsonResult(success, message, extraData));
        }

        public void JsonResult(string errMsg)
        {
            if (string.IsNullOrEmpty(errMsg))
                JsonSuccess();
            else
                JsonError(errMsg);
        }

        public void JsonSuccess()
        {
            JsonResult(true, null, null);
        }

        public void JsonError(string errMsg)
        {
            JsonResult(false, errMsg, null);
        }

        #endregion
    }
}