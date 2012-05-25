using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Oracle.DataAccess.Client;

namespace OracleDataAccess.Data
{
    public class PagingResult<T, C> : Interfaces.IPagingResult<T, C>
        where T : class, new()
        where C : ICollection<T>, new()
    {
        private C _Result;

        public C Result
        {
            get { return _Result; }
            set { _Result = value; }
        }

        private int _Total;

        public int Total
        {
            get { return _Total; }
            set { _Total = value; }
        }

        public PagingResult()
        {

        }

        public static Data.PagingResult<T, C> GetPagingList(IDbConnection conn, string tableName, string[] primaryKeys, int pageIndex, int pageSize, string[] fieldsOrder, bool isAsc, string where, params string[] fieldsShow)
        {
            PagingResult<T, C> ret = new PagingResult<T, C>();
            OracleParameter parTotal = new OracleParameter("TOTAL", OracleDbType.Int32, ParameterDirection.Output);
            using (IDataReader idr =
                SqlUtil.ExecuteProcedureReader(conn, "SP_PAGING",
                    new OracleParameter[]{new OracleParameter("P_TABLENAME", tableName),
                    new OracleParameter("P_PRIMARYKEYS", SqlScriptHandler.ArrayToString(primaryKeys,",",true)),
                    new OracleParameter("P_PAGEINDEX", pageIndex),
                    new OracleParameter("P_PAGESIZE", pageSize),
                    new OracleParameter("P_FIELDSSHOW", (fieldsShow==null)?"*":SqlScriptHandler.ArrayToString(fieldsShow,",",true)),
                    new OracleParameter("P_FIELDSORDER", fieldsOrder==null?string.Empty:string.Concat(SqlScriptHandler.ArrayToString(fieldsOrder,",",true),(!isAsc?" desc":""))),
                    new OracleParameter("P_WHERE", where),
                    new OracleParameter("RESULT", OracleDbType.RefCursor, ParameterDirection.Output),
                    parTotal
                    }))
            {
                ret.Result = DataMapping.ObjectHelper.FillCollection<T, C>(idr);
                ret.Total = Data.Converter.GetOracleParameterValue<int>(parTotal.Value);
            }
            if (ret.Total == 0)
            {
                ret.Total = ret.Result.Count;
            }
            return ret;
        }
    }
}
