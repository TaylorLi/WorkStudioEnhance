using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Oracle.DataAccess;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace OracleDataAccess
{
    public class Log
    {
        private static bool EnableLog = false;
        public static void InitLogging(string url)
        {
            EnableLog = true;
            DataMapping.Log.SetConfig(url);
        }

        public static string GetCommandMessage(System.Data.IDbCommand cmd)
        {
            if (cmd.CommandType == CommandType.StoredProcedure) return GenStroedProcedureMessage(cmd);
            string text = cmd.CommandText.ToLower();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("");
            if (text.IndexOf("insert ") >= 0 || text.IndexOf("update ") >= 0 || text.IndexOf("delete ") >= 0)
            {

                if (cmd.Parameters != null && cmd.Parameters.Count > 0)
                {
                    sb.Append("DECLARE ");
                    foreach (OracleParameter sp in cmd.Parameters)
                    {
                        sb.Append(sp.ParameterName).Append(" ").Append(GetLogType(sp));
                        if (sp.Direction == ParameterDirection.Input || sp.Direction == ParameterDirection.InputOutput)
                        {
                            sb.Append(" := ").Append(GetLogValue(sp)).Append(";");
                        }
                        else
                        {
                            sb.Append(";");
                        }
                    }
                }
                sb.Append("BEGIN ");

                sb.Append("EXECUTE IMMEDIATE '");
                sb.Append(cmd.CommandText.Replace("'", "''")).Append("'");

                if (cmd.Parameters != null && cmd.Parameters.Count > 0)
                {
                    sb.Append(" using ");
                    bool isFirst = true;
                    foreach (OracleParameter sp in cmd.Parameters)
                    {
                        if (isFirst)
                        {
                            isFirst = false;
                        }
                        else
                        {
                            sb.Append(",");
                        }
                        sb.Append(sp.ParameterName);
                    }
                }
                sb.Append(";");
                sb.Append("COMMIT;");
                sb.Append("END;");

            }
            else //for select;oracle execute immediate can not support multi-row ruturn select query
            {
                if (cmd.Parameters == null || cmd.Parameters.Count == 0)
                {
                    sb.Append(cmd.CommandText).Append(";");
                }
                else
                {
                    string cmdText = cmd.CommandText;
                    foreach (OracleParameter sp in cmd.Parameters)
                    {
                        cmdText = cmdText.Replace(string.Concat(":", sp.ParameterName), GetLogValue(sp));
                    }
                    sb.Append(cmdText).Append(";");
                }
            }
            return sb.ToString();
        }

        public static string GenStroedProcedureMessage(System.Data.IDbCommand cmd)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("");
            if (cmd.Parameters != null && cmd.Parameters.Count > 0)
            {
                sb.Append("DECLARE ");
                foreach (OracleParameter sp in cmd.Parameters)
                {
                    sb.Append(sp.ParameterName).Append(" ").Append(GetLogType(sp));
                    if (sp.Direction == ParameterDirection.Input || sp.Direction == ParameterDirection.InputOutput)
                    {
                        sb.Append(" := ").Append(GetLogValue(sp)).Append(";");
                    }
                    else
                    {
                        sb.Append(";");
                    }
                }
            }
            sb.Append("BEGIN ");
            sb.Append(cmd.CommandText);
            if (cmd.Parameters != null && cmd.Parameters.Count > 0)
            {
                sb.Append("(");
                bool isFirst = true;
                foreach (OracleParameter sp in cmd.Parameters)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        sb.Append(",");
                    }
                    sb.Append(sp.ParameterName).Append(" => ").Append(sp.ParameterName);
                }
                sb.Append(")");
            }
            sb.Append(";");
            sb.Append("END;");

            return sb.ToString();
        }

        public static string GetLogValue(OracleParameter sp)
        {
            bool appendN = false;
            bool appendSingleQuotes = false;
            string value = null;
            switch (sp.OracleDbType)
            {
                case OracleDbType.Array:
                    break;
                case OracleDbType.BFile:
                    break;
                case OracleDbType.BinaryDouble:
                    break;
                case OracleDbType.BinaryFloat:
                    break;
                case OracleDbType.Blob:
                    break;
                case OracleDbType.Byte:
                    break;
                case OracleDbType.Char:
                    appendSingleQuotes = true;
                    break;
                case OracleDbType.Clob:
                    break;
                case OracleDbType.Date:
                    value = string.Format("TO_DATE('{0}','yyyy-mm-dd hh24:mi:ss')", sp.Value.GetType() == typeof(DateTime) ? ((DateTime)sp.Value).ToString("yyyy-MM-dd HH:mm:ss") : sp.Value.ToString());
                    break;
                case OracleDbType.Decimal:
                    break;
                case OracleDbType.Double:
                    break;
                case OracleDbType.Int16:
                    break;
                case OracleDbType.Int32:
                    break;
                case OracleDbType.Int64:
                    break;
                case OracleDbType.IntervalDS:
                    break;
                case OracleDbType.IntervalYM:
                    break;
                case OracleDbType.Long:
                    break;
                case OracleDbType.LongRaw:
                    break;
                case OracleDbType.NChar:
                    appendSingleQuotes = true;
                    appendN = true;
                    break;
                case OracleDbType.NClob:
                    appendSingleQuotes = true;
                    appendN = true;
                    break;
                case OracleDbType.NVarchar2:
                    appendSingleQuotes = true;
                    appendN = true;
                    break;
                case OracleDbType.Object:
                    break;
                case OracleDbType.Raw:
                    break;
                case OracleDbType.Ref:
                    break;
                case OracleDbType.RefCursor:
                    break;
                case OracleDbType.Single:
                    break;
                case OracleDbType.TimeStamp:
                    value = string.Format("TO_TIMESTAMP('{0}','yyyy-mm-dd hh24:mi:ss.ff')", sp.Value.GetType() == typeof(DateTime) ? ((DateTime)sp.Value).ToString("yyyy-MM-dd HH:mm:ss.ff") : sp.Value.ToString());
                    break;
                case OracleDbType.TimeStampLTZ:
                    value = string.Format("TO_TIMESTAMP('{0}','yyyy-mm-dd hh24:mi:ss.ff')", sp.Value.GetType() == typeof(DateTime) ? ((DateTime)sp.Value).ToString("yyyy-MM-dd HH:mm:ss.ff") : sp.Value.ToString());
                    break;
                case OracleDbType.TimeStampTZ:
                    value = string.Format("TO_TIMESTAMP('{0}','yyyy-mm-dd hh24:mi:ss.ff')", sp.Value.GetType() == typeof(DateTime) ? ((DateTime)sp.Value).ToString("yyyy-MM-dd HH:mm:ss.ff") : sp.Value.ToString());
                    break;
                case OracleDbType.Varchar2:
                    appendSingleQuotes = true;
                    break;
                case OracleDbType.XmlType:
                    appendSingleQuotes = true;
                    appendN = true;
                    break;
                default:
                    break;
            }
            if (value != null)
                return value;
            else if (appendSingleQuotes && sp.Value != null)
            {
                return string.Concat(appendN ? "N" : "", "'", sp.Value.ToString().Replace("'", "''"), "'");
            }
            else
            {
                return sp.Value == null ? "NULL" : sp.Value.ToString();
            }
        }

        public static string GetLogType(OracleParameter sp)
        {
            bool appendType = false;
            string spType = sp.OracleDbType.ToString();
            switch (sp.OracleDbType)
            {
                case OracleDbType.Array:
                    break;
                case OracleDbType.BFile:
                    break;
                case OracleDbType.BinaryDouble:
                    spType = "BINARY_DOUBLE";
                    break;
                case OracleDbType.BinaryFloat:
                    spType = "BINARY_FLOAT";
                    break;
                case OracleDbType.Blob:
                    break;
                case OracleDbType.Byte:
                    spType = "CHAR(1)";
                    break;
                case OracleDbType.Char:
                    appendType = true;
                    break;
                case OracleDbType.Clob:
                    break;
                case OracleDbType.Date:
                    break;
                case OracleDbType.Decimal:
                    break;
                case OracleDbType.Double:
                    spType = "FLOAT";
                    break;
                case OracleDbType.Int16:
                    spType = "INTEGER";
                    break;
                case OracleDbType.Int32:
                    spType = "INTEGER";
                    break;
                case OracleDbType.Int64:
                    spType = "INTEGER";
                    break;
                case OracleDbType.IntervalDS:
                    spType = "INTERVAL DAY(2) TO SECOND(6)";
                    break;
                case OracleDbType.IntervalYM:
                    spType = "INTERVAL YEAR(2) TO MONTH";
                    break;
                case OracleDbType.Long:
                    break;
                case OracleDbType.LongRaw:
                    spType = "LONG RAW";
                    break;
                case OracleDbType.NChar:
                    appendType = true;
                    break;
                case OracleDbType.NClob:
                    break;
                case OracleDbType.NVarchar2:
                    appendType = true;
                    break;
                case OracleDbType.Object:
                    break;
                case OracleDbType.Raw:
                    break;
                case OracleDbType.Ref:
                    spType = "SYS_REFCURSOR";
                    break;
                case OracleDbType.RefCursor:
                    spType = "SYS_REFCURSOR";
                    break;
                case OracleDbType.Single:
                    spType = "FLOAT";
                    break;
                case OracleDbType.TimeStamp:
                    break;
                case OracleDbType.TimeStampLTZ:
                    spType = "TIMESTAMP(6) WITH LOCAL TIME ZONE";
                    break;
                case OracleDbType.TimeStampTZ:
                    spType = "TIMESTAMP(6) WITH TIME ZONE";
                    break;
                case OracleDbType.Varchar2:
                    appendType = true;
                    break;
                case OracleDbType.XmlType:
                    break;
                default:
                    break;
            }
            if (appendType)
            {
                int size = 0;
                if (sp.Value != null && sp.Value.GetType() == typeof(string))
                    size = sp.Value != null && sp.Value.ToString().Length > 0 ? sp.Value.ToString().Replace("'", "''").Length : 1;
                size = sp.Size > size ? sp.Size : (size == 0 ? 1 : size);
                return string.Concat(spType, "(", size, ")");
            }
            else
                return spType;
        }

        public static void LogCommand(System.Data.IDbCommand cmd)
        {
            string ctm = string.Empty;
            if (Config.CommandTimeout > 0)
            {
                if (EnableLog)
                {
                    ctm = string.Concat("Command timeout changed:", cmd.CommandTimeout, " > ", Config.CommandTimeout, "\r\n");
                }
                cmd.CommandTimeout = Config.CommandTimeout;
            }
            if (!EnableLog || cmd == null) return;
            string text = cmd.CommandText.ToLower();
            string msg = string.Concat(ctm, GetCommandMessage(cmd));
            if (cmd.CommandType == CommandType.StoredProcedure)
                StoredProcedure(msg);
            if (text.IndexOf("insert ") >= 0)
                Insert(msg);
            else if (text.IndexOf("update ") >= 0)
                Update(msg);
            else if (text.IndexOf("delete ") >= 0)
                Delete(msg);
            else
                Select(msg);
        }

        public static void Info(string l)
        {
            if (EnableLog && Config.Log.LogInfo) DataMapping.Log.WriteInfo(Config.Log.Logger.Info, l);
        }

        public static void StoredProcedure(string l)
        {
            if (EnableLog && Config.Log.LogStoredProcedure) DataMapping.Log.WriteInfo(Config.Log.Logger.StoredProcedure, l);
        }

        public static void Warning(string l)
        {
            if (EnableLog && Config.Log.LogWarning) DataMapping.Log.WriteWarn(Config.Log.Logger.Warning, l);
        }

        public static void Debug(string l)
        {
            if (EnableLog && Config.Log.LogDebug) DataMapping.Log.WriteInfo(Config.Log.Logger.Debug, l);
        }

        public static void Insert(string l)
        {
            if (EnableLog && Config.Log.LogInsert) DataMapping.Log.WriteInfo(Config.Log.Logger.Insert, l);
        }

        public static void Select(string l)
        {
            if (EnableLog && Config.Log.LogSelect) DataMapping.Log.WriteInfo(Config.Log.Logger.Select, l);
        }

        public static void Update(string l)
        {
            if (EnableLog && Config.Log.LogUpdate) DataMapping.Log.WriteInfo(Config.Log.Logger.Update, l);
        }

        public static void Delete(string l)
        {
            if (EnableLog && Config.Log.LogDelete) DataMapping.Log.WriteInfo(Config.Log.Logger.Delete, l);
        }
    }
}
