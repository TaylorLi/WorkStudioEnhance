using System;
using System.Collections.Generic;
using System.Text;
using OracleCodeBuilderLibrary.DataObjects;

namespace OracleCodeBuilderLibrary.BusinessObjects
{
    public class boTables
    {
        public static doTables.uoListTables GetTables(string connectionString)
        {
            using (System.Data.IDbConnection conn = OracleDataAccess.ConnectionHelper.CreateConnection(connectionString))
            {
                return DataMapping.ObjectHelper.FillCollection<doTables.uoTables, doTables.uoListTables>
                    (OracleDataAccess.SqlUtil.ExecuteReader(conn,
                    "SELECT UA.TABLE_NAME,UA.TABLE_TYPE,UA.COMMENTS FROM USER_TAB_COMMENTS UA ORDER BY UA.TABLE_NAME"));
            }
        }

        public static doTables.uoTables GetTable(string connectionString, string tableName)
        {
            using (System.Data.IDbConnection conn = OracleDataAccess.ConnectionHelper.CreateConnection(connectionString))
            {
                doTables.uoTables tb = DataMapping.ObjectHelper.FillObject<doTables.uoTables>
                    (OracleDataAccess.SqlUtil.ExecuteReader(conn, string.Format(
                    "SELECT UA.TABLE_NAME,UA.TABLE_TYPE,UA.COMMENTS FROM USER_TAB_COMMENTS UA where ua.TABLE_NAME='{0}'", tableName)));
                if (tb == null)
                {
                    tb = new doTables.uoTables();
                    tb.name = tableName;
                    return tb;
                }
                else
                    return tb;
            }
        }

    }
}
