using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary.WebObject
{

    public class OnlineUserTemplateMasterPage : System.Web.UI.MasterPage
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
            CommonLibrary.WebObject.SimultaneousLogin.Validate(Page, string.Concat(LoginPageUrl, "?ko=1&lang=" + CommonLibrary.Utility.MutiLanguage.GetLanguageString()));
            base.OnInit(e);
        }
    }
}
