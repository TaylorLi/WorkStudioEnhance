using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Oracle 的摘要说明
/// </summary>
public class OracleTemplate
{
    public static string GetSettingsPath()
    {
        return string.Concat(System.Web.HttpContext.Current.Server.MapPath("~/."), "\\Include\\Oracle\\");
    }

    public static OracleCodeBuilderLibrary.Settings GetSettingsByProjectName(string n)
    {
        OracleCodeBuilderLibrary.Settings s = new OracleCodeBuilderLibrary.Settings();
        s = s.FormXml(System.IO.File.ReadAllText(string.Concat(GetSettingsPath(), n)));
        return s;
    }
}
