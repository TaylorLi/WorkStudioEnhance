using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using OracleDataAccess;
using OracleDataAccess.Data;
using DataMapping;
using OracleCodeBuilderLibrary.DataObjects;

namespace OracleCodeBuilderLibrary.BusinessObjects
{
    public class boStoreProcedureParameters
    {
        public static doStoreProcedureParameters.uoListStoreProcedureParameters GetStoreProcedureParameters(string connectionString, string spName)
        {
            using (System.Data.IDbConnection conn = OracleDataAccess.ConnectionHelper.CreateConnection(connectionString))
            {
                string sql_spp = string.Format("SELECT tua.ARGUMENT_NAME,tua.DATA_TYPE,tua.POSITION,tua.SEQUENCE,tua.DEFAULT_VALUE,tua.IN_OUT from USER_ARGUMENTS tua where tua.OBJECT_NAME='{0}' order by tua.POSITION", spName.ToUpper());
                return DataMapping.ObjectHelper.FillCollection<doStoreProcedureParameters.uoStoreProcedureParameters, doStoreProcedureParameters.uoListStoreProcedureParameters>
                    (OracleDataAccess.SqlUtil.ExecuteReader(conn,
                    sql_spp));
            }
        }
    }
}
