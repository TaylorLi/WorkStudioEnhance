using System;
using System.Data;
using System.Configuration;
using Oracle.DataAccess;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace OracleDataAccess
{
    public class ConnectionHelper
    {
        public ConnectionHelper()
        {

        }
        public static IDbConnection CreateConnection(string connectionString)
        {
            IDbConnection cnn = new OracleConnection(connectionString);
            cnn.Open();
            return cnn;
        }

        public static IDbConnection CreateConnectionByKey(string key)
        {
            return CreateConnection(ConfigurationManager.ConnectionStrings[key].ConnectionString);
        }

        public static void DisposeConnection(IDbConnection conn)
        {
            conn.Close();
            conn.Dispose();
        }
    }
}
