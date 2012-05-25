using System;
using System.Collections.Generic;
using System.Text;
using OracleCodeBuilderLibrary.DataObjects;

namespace OracleCodeBuilderLibrary.BusinessObjects
{
    public class boStoreProcedures
    {
        public static doStoreProcedures.uoListStoreProcedures GetStoreProcedures(string connectionString)
        {
            using (System.Data.IDbConnection conn = OracleDataAccess.ConnectionHelper.CreateConnection(connectionString))
            {
                string sql_sp = "SELECT UP.OBJECT_NAME FROM USER_PROCEDURES UP ORDER BY UP.OBJECT_NAME";
                return DataMapping.ObjectHelper.FillCollection<doStoreProcedures.uoStoreProcedures, doStoreProcedures.uoListStoreProcedures>
                    (OracleDataAccess.SqlUtil.ExecuteReader(conn,
                    sql_sp));
            }
        }
    }
}
