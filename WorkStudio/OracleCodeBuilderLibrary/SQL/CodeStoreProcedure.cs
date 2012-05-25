using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using OracleCodeBuilderLibrary.BusinessObjects;
using OracleCodeBuilderLibrary.DataObjects;
using System.Text.RegularExpressions;

namespace OracleCodeBuilderLibrary.SQL
{
    public class CodeStoreProcedure
    {
        public static string GetStoreProcedureSimple(Settings st, string spName)
        {
            DataObjects.doStoreProcedureParameters.uoListStoreProcedureParameters spParameters = BusinessObjects.boStoreProcedureParameters.GetStoreProcedureParameters(st.ConnectionString, spName);
            StringBuilder sbParsCall = new StringBuilder();
            StringBuilder sbParsDeclare = new StringBuilder();
            for (int i = 0; i < spParameters.Count; i++)
            {
                DataObjects.doStoreProcedureParameters.uoStoreProcedureParameters p = spParameters[i];
                if (p.data_type == "REF CURSOR")
                    p.data_type = "SYS_REFCURSOR";
                sbParsDeclare.Append(string.Format("{0} {1}", p.name, p.data_type));
                List<string> mustDeclareLengthTypes = new List<string>(new string[] { "RAW", "CHAR", "NCHAR", "VARCHAR2", "NVARCHAR2" });
                if (mustDeclareLengthTypes.Contains(p.data_type))
                    sbParsDeclare.Append("(100)");
                string v = "";
                string mapColType = SQL.CodeScript.GetColumnType(p.data_type);
                if (p.IN_OUT == "IN" || p.IN_OUT == "IN/OUT")
                {
                    if (p.data_type != "REF CURSOR" && mapColType.Equals("System.String"))
                    {
                        v = "''";
                    }
                    else if (mapColType.Equals("System.Double") || mapColType.Equals("System.Single") || mapColType.Equals("System.Decimal") || mapColType.Equals("System.Int32"))
                    {
                        v = "0";
                    }
                    else if (mapColType.Equals("System.DateTime"))
                    {
                        v = "SYSDATE";
                    }
                    if (v != string.Empty)
                    {
                        sbParsDeclare.Append(" := ");
                        sbParsDeclare.Append(v);
                    }
                }
                sbParsDeclare.Append(";\t\t--Database type:").Append(p.data_type).Append(",").Append(p.IN_OUT);
                sbParsDeclare.Append("\r\n");
                if (i == 0)
                {
                    sbParsCall.AppendFormat("{0} => {0}", p.name);
                }
                else
                {
                    sbParsCall.Append("\t\t\t");
                    sbParsCall.AppendFormat(" {0} => {0}", p.name);
                }
                if (i != spParameters.Count - 1)
                {
                    sbParsCall.Append(",");
                    sbParsCall.Append("\r\n");
                }
            }
            StringBuilder sbRet = new StringBuilder();
            if (spParameters.Count > 0)
            {
                sbRet.AppendLine("DECLARE");
                sbRet.Append(sbParsDeclare.ToString());
            }
            sbRet.AppendLine("BEGIN");
            sbRet.Append("\t").Append(spName);
            if (spParameters.Count > 0)
            {
                sbRet.Append("(").Append(sbParsCall.ToString()).Append(");");
            }
            else
            {
                sbRet.Append(";");
            }
            sbRet.AppendLine("\r\nEND;");
            return sbRet.ToString();
        }

        public static string GetDataObject(Settings st, string objName, string spName, string simpleSql, string storeProcRemark)
        {
            using (IDbConnection conn = OracleDataAccess.ConnectionHelper.CreateConnection(st.ConnectionString))
            {
                List<OracleParameter> par = new List<OracleParameter>();
                List<OracleParameter> cursorPars = new List<OracleParameter>();
                Dictionary<string, string> outSimpleTypePars = new Dictionary<string, string>();
                Regex rVarDeclare = new Regex(@"DECLARE(?<g1>.*)\nBEGIN(?<g2>.*)\nEND;", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                string varDeclare = rVarDeclare.IsMatch(simpleSql) ? rVarDeclare.Match(simpleSql).Groups["g1"].Value : "";//变量定义部分
                foreach (doStoreProcedureParameters.uoStoreProcedureParameters storePar in boStoreProcedureParameters.GetStoreProcedureParameters(st.ConnectionString, spName))
                {
                    OracleParameter oraclePar;
                    string mapColType = SQL.CodeScript.GetColumnType(storePar.data_type);
                    object val = storePar.DEFAULT_VALUE;
                    int size = 0;
                    Regex rParam = new Regex(string.Concat(storePar.name, @" (?<gtype>.*)\s{0,1}:=\s{0,1}(?<gval>.*);"), RegexOptions.IgnoreCase);
                    string simpleVal = null;
                    if ((storePar.IN_OUT == "IN" || storePar.IN_OUT == "IN/OUT") && rParam.IsMatch(varDeclare))
                    {
                        simpleVal = rParam.Match(varDeclare).Groups["gval"].Value;
                    }
                    if (storePar.data_type == "REF CURSOR")
                    {
                        size = 0;
                    }
                    else if (mapColType.Equals("System.String"))
                    {
                        val = string.Empty;
                        if (!string.IsNullOrEmpty(simpleVal))
                        {
                            Regex rString = new Regex(@"'(?<gval>.*)'");
                            if (rString.IsMatch(simpleVal))
                                val = rString.Match(simpleVal).Groups["gval"];
                        }
                        size = 4000;
                    }
                    else if (mapColType.Equals("System.Double") || mapColType.Equals("System.Single") || mapColType.Equals("System.Decimal") || mapColType.Equals("System.Int32"))
                    {
                        val = 0.0;
                        if (!string.IsNullOrEmpty(simpleVal))
                        {
                            try
                            {
                                val = Convert.ChangeType(simpleVal, Type.GetType(mapColType));
                            }
                            catch
                            {
                                val = 0.0;
                            }
                        }
                    }
                    else if (mapColType.Equals("System.DateTime"))
                    {
                        val = DateTime.MinValue;
                        if (!string.IsNullOrEmpty(simpleVal))
                        {
                            if (simpleVal.ToUpper() == "SYSDATE")
                                val = DateTime.Now;
                            else
                            {
                                Regex rDate = new Regex(@"to_date\('(?<gVal>([0-9]|-|/){4,10})','(?<gFormat>([ymd]|-|/){4,10})'\)", RegexOptions.IgnoreCase);
                                if (rDate.IsMatch(simpleVal))
                                {
                                    try
                                    {
                                        Match m = rDate.Match(simpleVal);
                                        val = DateTime.ParseExact(m.Groups["gVal"].Value, m.Groups["gFormat"].Value, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                                    }
                                    catch
                                    {
                                        val = DateTime.MinValue;
                                    }
                                }
                            }
                        }
                    }
                    if (storePar.IN_OUT == "IN")
                    {
                        oraclePar = new OracleParameter(storePar.name, CodeScript.GetColumnOracleDbType(storePar.data_type), size, val, ParameterDirection.Input);
                    }
                    else
                    {
                        oraclePar = new OracleParameter(storePar.name, CodeScript.GetColumnOracleDbType(storePar.data_type), storePar.IN_OUT == "OUT" ? ParameterDirection.Output : (storePar.IN_OUT == "IN/OUT" ? ParameterDirection.InputOutput : ParameterDirection.ReturnValue));
                        if (size > 0)
                            oraclePar.Size = size;
                        if (storePar.IN_OUT == "IN/OUT")
                            oraclePar.Value = val;
                        if (oraclePar.OracleDbType == OracleDbType.RefCursor)
                            cursorPars.Add(oraclePar);
                        else
                            outSimpleTypePars.Add(storePar.name, mapColType);
                    }
                    par.Add(oraclePar);
                }
                IDataReader dr = OracleDataAccess.SqlUtil.ExecuteProcedureReader(conn, spName, par.ToArray());
                return GetDataObject(st, dr, objName, spName, par, cursorPars, outSimpleTypePars, storeProcRemark);
            }
        }

        public static string GetDataObject(Settings cf, IDataReader dr, string objName, string storeProcName, List<OracleParameter> allParams, List<OracleParameter> refCursorParams, Dictionary<string, string> outSimpleTypeParams, string storeProcRemark)
        {
            int index = 0;
            StringBuilder sbReturn = new StringBuilder();
            Dictionary<string, ColumnMapping.ColumnInfos> tableColumnInfos = new Dictionary<string, ColumnMapping.ColumnInfos>();

            foreach (OracleParameter p in refCursorParams)
            {
                if (index != 0)
                    dr.NextResult();
                tableColumnInfos[p.ParameterName] = ColumnMapping.GetColumnInfo(dr);
                index++;
            }

            sbReturn.AppendLine(string.Format(@"
//------------------------------------------------------------------------------
// <auto-generated>
//     Date time = {1}
//     This code was generated by tool,Version={0}.
//     Changes to this code may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------", Config.Version, DateTime.Now));
            sbReturn.AppendLine("using System;");
            sbReturn.AppendLine("using System.Collections.Generic;");
            sbReturn.AppendLine("using System.Text;");
            sbReturn.AppendLine("using OracleDataAccess.Data;");
            sbReturn.AppendLine("using DataMapping;");
            sbReturn.AppendLine("using System.Data;");
            sbReturn.AppendLine("using Oracle.DataAccess.Client;");
            sbReturn.AppendLine("using Oracle.DataAccess.Types;");
            sbReturn.AppendLine("");
            sbReturn.Append("namespace ").AppendLine(string.Concat(cf.DataObjectNameSpace, ".StoreProc"));
            sbReturn.AppendLine("{");
            StringBuilder sbObjects = new StringBuilder();
            StringBuilder sbAttributes = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in outSimpleTypeParams)
            {
                string key = SQL.Util.ConvertStyle(kv.Key, cf.DecorateStyle);
                sbAttributes.AppendLine("");
                sbAttributes.AppendLine(string.Format("\t\tprivate {0} _{1};", kv.Value, key));
                sbAttributes.AppendLine(string.Format("\t\tpublic {0} {1}", kv.Value, key));
                sbAttributes.AppendLine("\t\t{");
                sbAttributes.AppendLine("\t\t\tget");
                sbAttributes.AppendLine("\t\t\t{");
                sbAttributes.AppendLine(string.Format("\t\t\t\treturn this._{0};", key));
                sbAttributes.AppendLine("\t\t\t}");
                sbAttributes.AppendLine("\t\t\tset");
                sbAttributes.AppendLine("\t\t\t{");
                sbAttributes.AppendLine(string.Format("\t\t\t\tthis._{0} = value;", key));
                sbAttributes.AppendLine("\t\t\t}");
                sbAttributes.AppendLine("\t\t}");
            }
            foreach (string tableName in tableColumnInfos.Keys)
            {
                string decorateTableName = SQL.Util.ConvertStyle(tableName, cf.DecorateStyle);
                sbObjects.AppendLine("");
                sbObjects.AppendLine(GetSubDataObject(cf, decorateTableName, tableColumnInfos[tableName]));
                sbObjects.Append("\t\t").AppendLine(string.Format("public class {0}{1}: List<{2}{1}>", cf.UOListPrefix, decorateTableName, cf.UOPrefix));
                sbObjects.AppendLine("\t\t{");
                sbObjects.AppendLine(string.Format("\t\t\tpublic {0}{1}()", cf.UOListPrefix, decorateTableName));
                sbObjects.AppendLine("\t\t\t{");
                sbObjects.AppendLine("\t\t\t}");
                sbObjects.AppendLine("\t\t}");
                sbObjects.AppendLine("");

                sbAttributes.AppendLine("");
                sbAttributes.AppendLine(string.Format("\t\tprivate {0}{1} _{1};", cf.UOListPrefix, decorateTableName));
                sbAttributes.AppendLine(string.Format("\t\tpublic {0}{1} {1}", cf.UOListPrefix, decorateTableName));
                sbAttributes.AppendLine("\t\t{");
                sbAttributes.AppendLine("\t\t\tget");
                sbAttributes.AppendLine("\t\t\t{");
                sbAttributes.AppendLine(string.Format("\t\t\t\treturn this._{0};", decorateTableName));
                sbAttributes.AppendLine("\t\t\t}");
                sbAttributes.AppendLine("\t\t\tset");
                sbAttributes.AppendLine("\t\t\t{");
                sbAttributes.AppendLine(string.Format("\t\t\t\tthis._{0} = value;", decorateTableName));
                sbAttributes.AppendLine("\t\t\t}");
                sbAttributes.AppendLine("\t\t}");

            }
            if (!string.IsNullOrEmpty(storeProcRemark))
            {
                sbReturn.AppendLine("\t/// <summary>");
                sbReturn.AppendLine(string.Concat("\t///", "DO Unit:", storeProcRemark.Replace("\r\n", " ").Replace("\n", " ")));
                sbReturn.AppendLine("\t/// </summary>");
            }
            sbReturn.AppendLine(string.Format("\tpublic class {0}{1} : StoreProcBase<{0}{1}, {0}{1}.Results>", cf.DOPrefix, objName));
            sbReturn.AppendLine("\t{");
            sbReturn.AppendLine(string.Format("\t\tpublic {0}{1}()", cf.DOPrefix, objName));
            sbReturn.AppendLine("\t\t{");
            sbReturn.AppendLine(string.Format("\t\t\tStoreProcInfo = new StoreProcInformation({0}, \"{1}\");", cf.ConnectionKey, storeProcName));
            sbReturn.AppendLine("\t\t}");
            sbReturn.AppendLine("");
            sbReturn.AppendLine("#region Function");
            sbReturn.AppendLine("");
            sbReturn.AppendLine("\t\tpublic override Results GetResults(IDataParameter[] parameters)");
            sbReturn.AppendLine("\t\t{");
            sbReturn.AppendLine("\t\t\tusing(this.StoreProcInfo.Connection)");
            sbReturn.AppendLine("\t\t\t{");
            sbReturn.AppendLine(string.Format("\t\t\t\tResults results{0} = new Results();", objName));
            sbReturn.AppendLine("\t\t\t\tIDataReader dr = GetDataReader(parameters);");
            foreach (KeyValuePair<string, string> kv in outSimpleTypeParams)
            {
                string key = SQL.Util.ConvertStyle(kv.Key, cf.DecorateStyle);
                int parIndex = -1;
                for (int i = 0; i < allParams.Count; i++)
                {
                    if (allParams[i].ParameterName == kv.Key)
                    {
                        parIndex = i;
                        break;
                    }
                }
                Type parType = Type.GetType(kv.Value);
                sbReturn.AppendLine(string.Format("\t\t\t\t\t\tresults{0}.{1} = OracleDataAccess.Data.Converter.GetOracleParameterValue<{2}>(parameters[{3}].Value);", objName, key, kv.Value, parIndex));
            }
            index = 0;
            foreach (string tableName in tableColumnInfos.Keys)
            {
                string decorateTableName = SQL.Util.ConvertStyle(tableName, cf.DecorateStyle);
                if (index != 0)
                {
                    sbReturn.AppendLine("\t\t\t\tif (dr.NextResult())");
                    sbReturn.AppendLine("\t\t\t\t{");
                }
                sbReturn.AppendLine(string.Format("\t\t\t\t\t\tresults{0}.{1} = ObjectHelper.FillCollection<{2}{1}, {3}{1}>(dr);", objName, decorateTableName, cf.UOPrefix, cf.UOListPrefix));
                if (index != 0)
                {
                    sbReturn.AppendLine("\t\t\t\t}");
                }
                index++;
            }
            sbReturn.AppendLine(string.Format("\t\t\t\treturn results{0};", objName));
            sbReturn.AppendLine("\t\t\t}");
            sbReturn.AppendLine("\t\t}");
            sbReturn.AppendLine("");
            sbReturn.AppendLine("\t\tpublic override IDataReader GetDataReader(IDataParameter[] parameters)");
            sbReturn.AppendLine("\t\t{");
            sbReturn.AppendLine("\t\t\treturn OracleDataAccess.SqlUtil.ExecuteProcedureReader(StoreProcInfo.Connection, StoreProcInfo.StoreProcName, parameters);");
            sbReturn.AppendLine("\t\t}");
            sbReturn.AppendLine("");
            sbReturn.AppendLine("\t\tpublic override DataSet GetDataSet(IDataParameter[] parameters)");
            sbReturn.AppendLine("\t\t{");
            sbReturn.AppendLine("\t\t\tusing(this.StoreProcInfo.Connection)");
            sbReturn.AppendLine("\t\t\t{");
            sbReturn.AppendLine("\t\t\t\treturn OracleDataAccess.SqlUtil.ExecuteProcedureDataSet(StoreProcInfo.Connection, StoreProcInfo.StoreProcName, parameters);");
            sbReturn.AppendLine("\t\t\t}");
            sbReturn.AppendLine("\t\t}");
            sbReturn.AppendLine("");
            sbReturn.AppendLine("#endregion");
            sbReturn.AppendLine("");
            sbReturn.AppendLine("\t\tpublic class Results");
            sbReturn.AppendLine("\t\t\t{");
            sbReturn.AppendLine("\t\t\t\t#region Attributes");
            sbReturn.AppendLine(sbAttributes.ToString());
            sbReturn.AppendLine("\t\t\t\t#endregion");
            sbReturn.AppendLine("");
            sbReturn.AppendLine("\t\t\t\tpublic Results()");
            sbReturn.AppendLine("\t\t\t\t{");
            sbReturn.AppendLine("\t\t\t\t}");
            sbReturn.AppendLine("\t\t\t}");
            sbReturn.AppendLine("");
            sbReturn.AppendLine("\t\t#region Return Objects");
            sbReturn.Append(sbObjects.ToString());
            sbReturn.AppendLine("");
            sbReturn.AppendLine("\t\t#endregion");
            sbReturn.AppendLine("\t}");
            sbReturn.AppendLine("}");
            return sbReturn.ToString();
        }
        public static string GetSubDataObject(Settings cf, string key, ColumnMapping.ColumnInfos columnInfos)
        {
            StringBuilder sbReturn = new StringBuilder();
            sbReturn.AppendLine(string.Format("\t\tpublic class {0}{1}", cf.UOPrefix, key));
            sbReturn.AppendLine("\t\t{");
            sbReturn.AppendLine(string.Format("\t\t\tpublic {0}{1}()", cf.UOPrefix, key));
            sbReturn.AppendLine("\t\t\t{");
            sbReturn.AppendLine("\t\t\t}");
            StringBuilder sb_columns = new StringBuilder();
            sb_columns.AppendLine("\t\t\t#region Columns");
            foreach (ColumnMapping.ColumnInfo c in columnInfos)
            {
                string decorateColumnName = SQL.Util.ConvertStyle(c.Column, cf.DecorateStyle);
                sb_columns.AppendLine(string.Format("\t\t\tprivate {0} _{1};", c.ColumnType, decorateColumnName));
                sb_columns.Append(string.Format("\t\t\t[Mapping(\"{0}", c.Column));
                sb_columns.AppendLine("\")]");
                sb_columns.AppendLine(string.Format("\t\t\tpublic {0} {1}", c.ColumnType, decorateColumnName));
                sb_columns.AppendLine("\t\t\t{");
                sb_columns.AppendLine("\t\t\t\tget");
                sb_columns.AppendLine("\t\t\t\t{");
                sb_columns.AppendLine(string.Format("\t\t\t\t\treturn _{0};", decorateColumnName));
                sb_columns.AppendLine("\t\t\t\t}");
                sb_columns.AppendLine("\t\t\t\tset");
                sb_columns.AppendLine("\t\t\t\t{");
                sb_columns.AppendLine(string.Format("\t\t\t\t\t_{0} = value;", decorateColumnName));
                sb_columns.AppendLine("\t\t\t\t}");
                sb_columns.AppendLine("\t\t\t}");
            }
            sb_columns.AppendLine("\t\t\t#endregion");

            sbReturn.AppendLine(sb_columns.ToString());
            sbReturn.Append("\t\t}");
            return sbReturn.ToString();
        }
        public static string GetBusinessObject(Settings cf, string connectionString, string objName, string storeProcName, string storeProcRemark)
        {
            StringBuilder sbArgs = new StringBuilder();
            StringBuilder sbArgsValues = new StringBuilder();
            StringBuilder sbParameters = new StringBuilder();
            sbParameters.AppendLine("\t\t\tList<IDataParameter> parameters = new List<IDataParameter>();");
            sbParameters.AppendLine("");
            foreach (DataObjects.doStoreProcedureParameters.uoStoreProcedureParameters p in BusinessObjects.boStoreProcedureParameters.GetStoreProcedureParameters(connectionString, storeProcName))
            {
                string decorateParamName = SQL.Util.ConvertStyle(p.name, cf.DecorateStyle);
                string camelParamName = SQL.Util.ConvertStyle(p.name, CodeDecorateStyle.Camel);
                if (p.IN_OUT != "OUT")
                {
                    if (sbArgs.Length > 0) sbArgs.Append(", ");
                    if (sbArgsValues.Length > 0) sbArgsValues.Append(", ");
                    sbArgsValues.Append(camelParamName);
                    sbArgs.Append(CodeScript.GetColumnType(p.data_type)).Append(" ").Append(camelParamName);
                }
                if (p.IN_OUT == "IN")
                {
                    sbParameters.AppendLine(string.Format("\t\t\tparameters.Add(new Oracle.DataAccess.Client.OracleParameter(\"{0}\", Oracle.DataAccess.Client.OracleDbType.{1}, {2}, ParameterDirection.Input));", p.name, CodeScript.GetColumnOracleDbType(p.data_type), camelParamName));
                }
                else if (p.IN_OUT == "OUT")
                {
                    sbParameters.AppendLine(string.Format("\t\t\tparameters.Add(new Oracle.DataAccess.Client.OracleParameter(\"{0}\", Oracle.DataAccess.Client.OracleDbType.{1}{2}, ParameterDirection.Output));", p.name, CodeScript.GetColumnOracleDbType(p.data_type),
                       SQL.CodeScript.GetColumnType(p.data_type).Equals("System.String") ? ", 4000 ,null" : ""));
                }
                else if (p.IN_OUT == "IN/OUT")
                {
                    sbParameters.AppendLine(string.Format("\t\t\tparameters.Add(new Oracle.DataAccess.Client.OracleParameter(\"{0}\", Oracle.DataAccess.Client.OracleDbType.{1}{2}, {3}, ParameterDirection.InputOutput));", p.name, CodeScript.GetColumnOracleDbType(p.data_type),
                       SQL.CodeScript.GetColumnType(p.data_type).Equals("System.String") ? ", 4000" : "", camelParamName));
                }
                else
                {
                    sbParameters.AppendLine(string.Format("\t\t\tparameters.Add(new Oracle.DataAccess.Client.OracleParameter(\"{0}\", Oracle.DataAccess.Client.OracleDbType.{1}, ParameterDirection.ReturnValue));", p.name, CodeScript.GetColumnOracleDbType(p.data_type)));
                }
                //if (CodeScript.GetColumnType(p.data_type).Equals("DateTime"))
                //{
                //    sbParameters.AppendLine(string.Format("\t\t\tparameters.Add(new Oracle.DataAccess.Client.OracleParameter(\"{0}\", {1}.ToString(\"yyyyMMdd\", System.Globalization.DateTimeFormatInfo.InvariantInfo)));", p.name, paramName));
                //}
                //else
                //{
                //    sbParameters.AppendLine(string.Format("\t\t\tparameters.Add(new Oracle.DataAccess.Client.OracleParameter(\"{0}\", {1}));", p.name, paramName));
                //}
            }
            StringBuilder sbReturn = new StringBuilder();
            sbReturn.AppendLine("using System;");
            sbReturn.AppendLine("using System.Collections.Generic;");
            sbReturn.AppendLine("using System.Text;");
            sbReturn.AppendLine("using System.Data;");
            sbReturn.AppendLine("using OracleDataAccess;");
            sbReturn.AppendLine("using OracleDataAccess.Data;");
            sbReturn.AppendLine("using DataMapping;");
            sbReturn.AppendLine("using Oracle.DataAccess.Client;");
            sbReturn.AppendLine("using Oracle.DataAccess.Types;");
            sbReturn.Append("using ").Append(string.Concat(cf.DataObjectNameSpace, ".StoreProc")).AppendLine(";");
            sbReturn.AppendLine("");

            sbReturn.Append("namespace ").AppendLine(string.Concat(cf.BusinessObjectNameSpace, ".StoreProc"));
            sbReturn.AppendLine("{");
            if (!string.IsNullOrEmpty(storeProcRemark))
            {
                sbReturn.AppendLine("\t/// <summary>");
                sbReturn.AppendLine(string.Concat("\t///", "DO Unit:", storeProcRemark.Replace("\r\n", " ").Replace("\n", " ")));
                sbReturn.AppendLine("\t/// </summary>");
            }
            sbReturn.AppendLine(string.Format("\tpublic class {0}{1}", cf.BOPrefix, objName));
            sbReturn.AppendLine("\t{");
            sbReturn.Append("\t\t#region This source code was auto-generated by tool,Version=").AppendLine(Config.Version);

            sbReturn.AppendLine(string.Format(@"
                //------------------------------------------------------------------------------
                // <auto-generated>
                //     Date time = {1}
                //     This code was generated by tool,Version={0}.
                //     Changes to this code may cause incorrect behavior and will be lost if
                //     the code is regenerated.
                // </auto-generated>
                //------------------------------------------------------------------------------", Config.Version, DateTime.Now));
            sbReturn.AppendLine("");
            sbReturn.AppendLine("\t\t/// <summary>");
            sbReturn.AppendLine("\t\t/// Get parameters");
            sbReturn.AppendLine("\t\t/// </summary>");
            sbReturn.AppendLine(string.Format("\t\tpublic static IDataParameter[] GetParameters({0})", sbArgs.ToString()));
            sbReturn.AppendLine("\t\t{");
            sbReturn.AppendLine(sbParameters.ToString());
            sbReturn.AppendLine("\t\t\treturn parameters.ToArray();");
            sbReturn.AppendLine("\t\t}");
            sbReturn.AppendLine("");
            sbReturn.AppendLine("\t\t/// <summary>");
            sbReturn.AppendLine("\t\t/// Get object result");
            sbReturn.AppendLine("\t\t/// </summary>");
            sbReturn.AppendLine(string.Format("\t\tpublic static {0}{1}.Results GetResults({3}{2})", cf.DOPrefix, objName, sbArgs.ToString(), cf.IsPassConnectionStringToBusiness ? "string connString, " : ""));
            sbReturn.AppendLine("\t\t{");
            sbReturn.AppendLine(string.Format("\t\t\t{0}{1} da = new {0}{1}();", cf.DOPrefix, objName));
            if (cf.IsPassConnectionStringToBusiness)
            {
                sbReturn.AppendLine("\t\t\tda.StoreProcInfo.ConnectionString = connString;");
            }
            sbReturn.AppendLine(string.Format("\t\t\treturn da.GetResults(GetParameters({0}));", sbArgsValues.ToString()));
            sbReturn.AppendLine("\t\t}");
            sbReturn.AppendLine("");
            sbReturn.AppendLine("\t\t/// <summary>");
            sbReturn.AppendLine("\t\t/// Get DataSet result");
            sbReturn.AppendLine("\t\t/// </summary>");
            sbReturn.AppendLine(string.Format("\t\tpublic static DataSet GetDataSet({1}{0})", sbArgs.ToString(), cf.IsPassConnectionStringToBusiness ? "string connString, " : ""));
            sbReturn.AppendLine("\t\t{");
            sbReturn.AppendLine(string.Format("\t\t\t{0}{1} da = new {0}{1}();", cf.DOPrefix, objName));
            if (cf.IsPassConnectionStringToBusiness)
            {
                sbReturn.AppendLine("\t\t\tda.ConnInfo.ConnectionString = connString;");
            }
            sbReturn.AppendLine(string.Format("\t\t\treturn da.GetDataSet(GetParameters({0}));", sbArgsValues.ToString()));
            sbReturn.AppendLine("\t\t}");
            sbReturn.AppendLine("");
            sbReturn.AppendLine("\t\t#endregion");
            sbReturn.AppendLine("");
            sbReturn.AppendLine("\t\t#region User extensions");
            sbReturn.AppendLine("");
            sbReturn.AppendLine("\t\t#endregion");
            sbReturn.AppendLine("\t}");
            sbReturn.AppendLine("}");
            return sbReturn.ToString();
        }
    }
}
