/* 
 * Global页面基础类
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
    public class TemplateGlobal : System.Web.HttpApplication
    {
        protected void Application_Start_Base(object sender, EventArgs e)
        {

        }

        protected void Application_Error_Base(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.RawUrl) && Request.RawUrl.ToUpper().IndexOf("ABOUT_DLL.ASPX") > 0)
            {
                Response.Write(new CommonLibrary.Entities.LibraryInfos(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\Bin\\"), string.IsNullOrEmpty(Request["sp"]) ? "*" : Request["sp"]).ToString());
                Response.End();
                if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Server != null)
                {
                    System.Web.HttpContext.Current.Server.ClearError();
                }
            }
        }

        public void Session_OnStart_Base(object sender, EventArgs e)
        {

        }

        public void Application_BeginRequest_Base(object sender, EventArgs e)
        {

        }

        public void Session_End_Base(object sender, EventArgs e)
        {
            CommonLibrary.WebObject.SimultaneousLogin.ValidateOnSessionEnd(Application, Session);
        }
    }
}
