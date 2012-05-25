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
using OracleCodeBuilderLibrary.DataObjects;
using System.Text.RegularExpressions;

public partial class Oracle_Edit : System.Web.UI.Page
{
    private string _DO_String;

    public string DO_String
    {
        get { return _DO_String; }
        set { _DO_String = value; }
    }

    private string _DO_String_Display;

    public string DO_String_Display
    {
        get { return _DO_String_Display; }
        set { _DO_String_Display = value; }
    }


    private string _BO_String;

    public string BO_String
    {
        get { return _BO_String; }
        set { _BO_String = value; }
    }

    public string SplitHeader
    {
        get
        {
            if (CommonLibrary.WebObject.WebHelper.IsFireFox(Page) || Request.Browser.Type == "Desktop")
            {
                return "<div style='width:100%;height:45px;'></div>";
            }
            else
                return "<div style='width:100%;height:15px;'></div>";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //DO_String = GetString();

        //DO_String_Display = DO_String;// new HtmlCodeColor.CodeColor(new HtmlCodeColor.CSharpAdapter()).ColorCode(DO_String);

        //BO_String = GetString1();

        //OracleCodeBuilderLibrary.Settings st = new OracleCodeBuilderLibrary.Settings();
        //st.ConnectionString = "Data Source=.;Initial Catalog=WTMS;Persist Security Info=True;User ID=sa;Password=gzuat";

        //System.IO.File.WriteAllText(OracleTemplate.GetSettingsPath() + st.ProjectName, st.ToXml());


    }

    #region Testing

    public string GetString()
    {
        OracleCodeBuilderLibrary.Settings cf = new OracleCodeBuilderLibrary.Settings();
        OracleCodeBuilderLibrary.DataObjects.doColumns.uoListColumns cs = new OracleCodeBuilderLibrary.DataObjects.doColumns.uoListColumns();
        OracleCodeBuilderLibrary.DataObjects.doColumns.uoColumns c = new OracleCodeBuilderLibrary.DataObjects.doColumns.uoColumns();
        OracleCodeBuilderLibrary.DataObjects.doTables.uoTables tableObj = OracleCodeBuilderLibrary.BusinessObjects.boTables.GetTable(cf.ConnectionString, "USER_TAB_COMMENTS");
        doPKs.uoListPKs listPks = OracleCodeBuilderLibrary.BusinessObjects.boPks.GetPrimaryKeys(cf.ConnectionString, "spt_values");
        doColumns.uoListColumns listColumns = OracleCodeBuilderLibrary.BusinessObjects.boColumns.GetColumns(cf.ConnectionString, "spt_values");
        doRemarks.uoListRemarks listRemarks = OracleCodeBuilderLibrary.BusinessObjects.boRemarks.GetRemarks(cf.ConnectionString, "spt_values");
        return OracleCodeBuilderLibrary.SQL.CodeScript.GetDataObject(cf, tableObj, listColumns, listPks, listRemarks, null);
    }

    public string GetString1()
    {
        OracleCodeBuilderLibrary.Settings cf = new OracleCodeBuilderLibrary.Settings();
        OracleCodeBuilderLibrary.DataObjects.doTables.uoTables tableObj = OracleCodeBuilderLibrary.BusinessObjects.boTables.GetTable(cf.ConnectionString, "USER_TAB_COMMENTS");
        OracleCodeBuilderLibrary.DataObjects.doColumns.uoListColumns cs = new OracleCodeBuilderLibrary.DataObjects.doColumns.uoListColumns();
        OracleCodeBuilderLibrary.DataObjects.doColumns.uoColumns c = new OracleCodeBuilderLibrary.DataObjects.doColumns.uoColumns();
        doPKs.uoListPKs listPks = OracleCodeBuilderLibrary.BusinessObjects.boPks.GetPrimaryKeys(cf.ConnectionString, "spt_values");
        doColumns.uoListColumns listColumns = OracleCodeBuilderLibrary.BusinessObjects.boColumns.GetColumns(cf.ConnectionString, "spt_values");
        doRemarks.uoListRemarks listRemarks = OracleCodeBuilderLibrary.BusinessObjects.boRemarks.GetRemarks(cf.ConnectionString, "spt_values");
        return OracleCodeBuilderLibrary.SQL.CodeScript.GetBusinessObject(cf, tableObj, listColumns, listPks);
    }

    #endregion
}


