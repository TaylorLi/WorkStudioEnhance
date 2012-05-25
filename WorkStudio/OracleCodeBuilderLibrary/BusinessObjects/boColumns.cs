using System;
using System.Collections.Generic;
using System.Text;
using OracleCodeBuilderLibrary.DataObjects;

namespace OracleCodeBuilderLibrary.BusinessObjects
{
    public class boColumns
    {
        public static doColumns.uoListColumns GetColumns(string connectionString, string tableName)
        {
            string sql_column = string.Format("SELECT a.COLUMN_NAME,a.DATA_TYPE,a.DATA_LENGTH,a.DATA_PRECISION,a.DATA_SCALE,a.NULLABLE,a.DATA_DEFAULT from USER_TAB_COLUMNS a WHERE a.TABLE_NAME='{0}' ORDER BY COLUMN_ID", tableName);

            using (System.Data.IDbConnection conn = OracleDataAccess.ConnectionHelper.CreateConnection(connectionString))
            {
                return DataMapping.ObjectHelper.FillCollection<doColumns.uoColumns, doColumns.uoListColumns>(OracleDataAccess.SqlUtil.ExecuteReader(conn, sql_column));
            }
        }
    }
}
