/* 
 * Master页面基类
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
    public class TemplateMasterPage : System.Web.UI.MasterPage
    {
        private string _LoginPageUrl = "~/Login.aspx";
        public string LoginPageUrl
        {
            get { return _LoginPageUrl; }
            set { _LoginPageUrl = value; }
        }

        override protected void OnInit(EventArgs e)
        {
            //if (Global.EnableSimultaneousLogin > 0)
            CommonLibrary.WebObject.SimultaneousLogin.Validate(Page, string.Concat(LoginPageUrl, "?killout=1&lang=", CommonLibrary.Utility.MutiLanguage.GetLanguageString()));
            base.OnInit(e);
        }

        //protected override void OnLoad(EventArgs e)
        //{
        //    if (ADHolidaysLibrary.SessionHelper.GetProfiles() != null && ADHolidaysLibrary.SessionHelper.GetProfiles().User.IsChangePassword)
        //    {
        //        Response.Redirect("~/Admin/ChangePassword.aspx");
        //    }
        //    base.OnLoad(e);
        //}

        public void ResponseOfNoRights()
        {
            Response.Write("您无权访问该页面。");
            Response.End();
        }

        public void CheckRight(bool allowed)
        {
            if (!allowed) ResponseOfNoRights();
        }
    }
}
