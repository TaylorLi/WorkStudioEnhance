using System;
using System.Collections.Generic;
using System.Text;
using OracleCodeBuilderLibrary.DataObjects;

namespace OracleCodeBuilderLibrary.BusinessObjects
{
    public class boRemarks
    {
        public static doRemarks.uoListRemarks GetRemarks(string connectionString, string tableName)
        {
            string sql_rm = string.Empty;
            sql_rm = "SELECT A.COLUMN_NAME,A.COMMENTS FROM USER_COL_COMMENTS A WHERE A.TABLE_NAME='{0}'";
            sql_rm = string.Format(sql_rm, tableName);
            using (System.Data.IDbConnection conn = OracleDataAccess.ConnectionHelper.CreateConnection(connectionString))
            {
                return DataMapping.ObjectHelper.FillCollection<doRemarks.uoRemarks, doRemarks.uoListRemarks>
                    (OracleDataAccess.SqlUtil.ExecuteReader(conn, sql_rm));
            }
        }
    }
}
