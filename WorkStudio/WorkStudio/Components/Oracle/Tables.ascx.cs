﻿using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Components_Oracle_Tables : System.Web.UI.UserControl
{
    private string _ConnectionString;

    public string ConnectionString
    {
        get { return _ConnectionString; }
        set { _ConnectionString = value; }
    }

    private string _Error;

    public string Error
    {
        get { return _Error; }
        set { _Error = value; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            rptResult.DataSource = OracleCodeBuilderLibrary.BusinessObjects.boTables.GetTables(ConnectionString);
            rptResult.DataBind();
        }
        catch (Exception ex)
        {
            Error = ex.Message;
            WorkStudioLibrary.Logging.Files.Log.Error(ex);
        }
    }
}
