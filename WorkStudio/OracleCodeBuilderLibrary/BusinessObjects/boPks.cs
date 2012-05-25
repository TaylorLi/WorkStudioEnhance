using System;
using System.Collections.Generic;
using System.Text;
using OracleCodeBuilderLibrary.DataObjects;

namespace OracleCodeBuilderLibrary.BusinessObjects
{
    public class boPks
    {
        public static doPKs.uoListPKs GetPrimaryKeys(string connectionString, string tableName)
        {
            string sql_pk = string.Format("SELECT COLUMN_NAME FROM USER_CONS_COLUMNS  WHERE CONSTRAINT_NAME   =   (SELECT   CONSTRAINT_NAME   FROM   USER_CONSTRAINTS WHERE TABLE_NAME = '{0}' AND CONSTRAINT_TYPE   ='P')", tableName);
            using (System.Data.IDbConnection conn = OracleDataAccess.ConnectionHelper.CreateConnection(connectionString))
            {
                return DataMapping.ObjectHelper.FillCollection<doPKs.uoPKs, doPKs.uoListPKs>
                    (OracleDataAccess.SqlUtil.ExecuteReader(conn, sql_pk));

            }
        }
    }
}
