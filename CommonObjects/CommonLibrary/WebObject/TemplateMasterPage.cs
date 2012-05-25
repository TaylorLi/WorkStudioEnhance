/* 
 * Masterҳ�����
 *  
 * �������ż���   
 * ʱ�䣺2011��9��5��
 * 
 * �޸ģ�[����]         
 * ʱ�䣺[ʱ��]             
 * ˵����[˵��]
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
            Response.Write("����Ȩ���ʸ�ҳ�档");
            Response.End();
        }

        public void CheckRight(bool allowed)
        {
            if (!allowed) ResponseOfNoRights();
        }
    }
}
