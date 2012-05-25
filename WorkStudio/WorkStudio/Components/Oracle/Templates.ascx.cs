using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Components_Oracle_Templates : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        rptResult.DataSource = GetTemplates();
        rptResult.DataBind();
    }


    public static Templates GetTemplates()
    {
        Templates ts = new Templates();
        foreach (string s in System.IO.Directory.GetFiles(OracleTemplate.GetSettingsPath()))
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(s);
            OracleCodeBuilderLibrary.Settings st = new OracleCodeBuilderLibrary.Settings();
            st = st.FormXml(System.IO.File.ReadAllText(s));
            if (st != null)
            {
                ts.Add(st);
            }
        }
        return ts;
    }
}

public class Templates : CommonLibrary.ObjectBase.ListBase<OracleCodeBuilderLibrary.Settings>
{
    public Templates()
    {

    }
}

