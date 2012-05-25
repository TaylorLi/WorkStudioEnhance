using System;
using System.Collections.Generic;
using System.Text;
using OracleCodeBuilderLibrary.DataObjects;
using System.Text.RegularExpressions;
using Oracle.DataAccess.Types;
using Oracle.DataAccess.Client;

namespace OracleCodeBuilderLibrary.SQL
{
    /// <summary>
    /// From Table or view script
    /// </summary>
    public static class CodeScript
    {
        #region Type Convert

        public static OracleDbType GetColumnOracleDbType(string type)
        {
            OracleDbType reval;
            Regex r = new Regex(@"\(.*?\)");
            type = r.Replace(type, "");
            switch (type.ToUpper())
            {
                case "BFILE":
                    reval = OracleDbType.BFile;
                    break;
                case "BLOB":
                    reval = OracleDbType.Blob;
                    break;
                case "CHAR":
                    reval = OracleDbType.Char;
                    break;
                case "CLOB":
                    reval = OracleDbType.Clob;
                    break;
                case "DATE":
                    reval = OracleDbType.Date;
                    break;
                case "FLOAT":
                    reval = OracleDbType.Single;
                    break;
                case "INTEGER":
                    reval = OracleDbType.Int32;
                    break;
                case "INTERVAL DAY TO SECOND":
                    reval = OracleDbType.IntervalDS;
                    break;
                case "INTERVAL YEAR TO MONTH":
                    reval = OracleDbType.IntervalYM;
                    break;
                case "LONG":
                    reval = OracleDbType.Long;
                    break;
                case "LONG RAW":
                    reval = OracleDbType.LongRaw;
                    break;
                case "NCHAR":
                    reval = OracleDbType.NChar;
                    break;
                case "NCLOB":
                    reval = OracleDbType.NClob;
                    break;
                case "NUMBER":
                    reval = OracleDbType.Decimal;
                    break;
                case "NVARCHAR2":
                    reval = OracleDbType.NVarchar2;
                    break;
                case "RAW":
                    reval = OracleDbType.Raw;
                    break;
                case "ROWID":
                    reval = OracleDbType.Varchar2;
                    break;
                case "TIMESTAMP":
                    reval = OracleDbType.TimeStamp;
                    break;
                case "TIMESTAMP WITH LOCAL TIME ZONE":
                    reval = OracleDbType.TimeStampLTZ;
                    break;
                case "TIMESTAMP WITH TIME ZONE":
                    reval = OracleDbType.TimeStampTZ;
                    break;
                case "UNSIGNED INTEGER":
                    reval = OracleDbType.Int32;
                    break;
                case "VARCHAR2":
                    reval = OracleDbType.Varchar2;
                    break;
                case "BINARY_DOUBLE":
                    reval = OracleDbType.BinaryDouble;
                    break;
                case "BINARY_FLOAT":
                    reval = OracleDbType.BinaryFloat;
                    break;
                case "REF CURSOR":
                    reval = OracleDbType.RefCursor;
                    break;
                default:
                    reval = OracleDbType.Varchar2;
                    break;
            }
            return reval;
        }

        public static string GetColumnType(string type)
        {
            string reval;
            Regex r = new Regex(@"\(.*?\)");
            type = r.Replace(type, "");
            switch (type.ToUpper())
            {
                case "BFILE":
                    reval = "System.Byte[]";
                    break;
                case "BLOB":
                    reval = "System.Byte[]";
                    break;
                case "CHAR":
                    reval = "System.String";
                    break;
                case "CLOB":
                    reval = "System.String";
                    break;
                case "DATE":
                    reval = "System.DateTime";
                    break;
                case "FLOAT":
                    //reval = "System.Single";
                    reval = "System.Double";
                    break;
                case "INTEGER":
                    reval = "System.Int32";
                    break;
                case "INTERVAL DAY TO SECOND":
                    reval = "TimeSpan";
                    break;
                case "INTERVAL YEAR TO MONTH":
                    reval = "System.String";
                    break;
                case "LONG":
                    reval = "System.String";
                    break;
                case "LONG RAW":
                    reval = "System.Byte[]";
                    break;
                case "NCHAR":
                    reval = "System.String";
                    break;
                case "NCLOB":
                    reval = "System.String";
                    break;
                case "NUMBER":
                    reval = "System.Decimal";
                    break;
                case "NVARCHAR2":
                    reval = "System.String";
                    break;
                case "RAW":
                    reval = "System.Byte[]";
                    break;
                case "ROWID":
                    reval = "System.String";
                    break;
                case "TIMESTAMP":
                    reval = "System.DateTime";
                    break;
                case "TIMESTAMP WITH LOCAL TIME ZONE":
                    reval = "System.DateTime";
                    break;
                case "TIMESTAMP WITH TIME ZONE":
                    reval = "System.DateTime";
                    break;
                case "UNSIGNED INTEGER":
                    reval = "System.UInt32";
                    break;
                case "VARCHAR2":
                    reval = "System.String";
                    break;
                case "BINARY_DOUBLE":
                    reval = "System.Double";
                    break;
                case "BINARY_FLOAT":
                    //reval = "System.Single";
                    reval = "System.Double";
                    break;
                case "REF CURSOR":
                    reval = "object";
                    break;
                default:
                    reval = "System.String";
                    break;
            }
            return reval;
        }

        public static string GetColumnType(doColumns.uoListColumns columns, string column)
        {
            foreach (doColumns.uoColumns c in columns)
            {
                if (c.column_name.Equals(column))
                {
                    return GetColumnType(c.data_type);
                }
            }
            return "System.String";
        }

        #endregion

        #region Generate DO,BO

        public static string GetDataObject(Settings cf, doTables.uoTables table, doColumns.uoListColumns listColumn, doPKs.uoListPKs listPK, doRemarks.uoListRemarks listRemarks, Dictionary<string, DataMapping.FieldGenerateInfo> fieldsGeneration)
        {
            string tableName = table.name;
            string decorateTableName = Util.ConvertStyle(tableName, cf.DecorateStyle);
            StringBuilder sb_ret = new StringBuilder();
            sb_ret.AppendLine(string.Format(@"
//------------------------------------------------------------------------------
// <auto-generated>
//     Date time = {1}
//     This code was generated by tool,Version={0}.
//     Changes to this code may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------", Config.Version, DateTime.Now));
            sb_ret.AppendLine("using System;");
            sb_ret.AppendLine("using System.Collections.Generic;");
            sb_ret.AppendLine("using System.Text;");
            sb_ret.AppendLine("using OracleDataAccess.Data;");
            sb_ret.AppendLine("using DataMapping;");
            sb_ret.AppendLine("");
            sb_ret.Append("namespace ").AppendLine(cf.DataObjectNameSpace);
            sb_ret.AppendLine("{");
            if (!string.IsNullOrEmpty(table.Comments))
            {
                sb_ret.AppendLine("\t/// <summary>");
                sb_ret.AppendLine(string.Concat("\t///", "DO Unit:", table.Comments.Replace("\r\n", " ").Replace("\n", " ")));
                sb_ret.AppendLine("\t/// </summary>");
            }
            sb_ret.AppendLine(string.Format("\tpublic class {1}{0} : DOBase<{1}{0}.{2}{0}, {1}{0}.{3}{0}>", decorateTableName, cf.DOPrefix, cf.UOPrefix, cf.UOListPrefix));
            sb_ret.AppendLine("\t{");
            sb_ret.AppendLine("\t\tpublic enum Columns");
            sb_ret.AppendLine("\t\t{");
            StringBuilder sb_columns = new StringBuilder();
            sb_columns.AppendLine("\t\t\t#region Columns");
            StringBuilder sbPks = new StringBuilder();
            foreach (doPKs.uoPKs p in listPK)
            {
                if (p.name.Trim().IndexOf(" ") > 0) p.name = p.name.Trim().Replace(" ", "_space_");
                if (sbPks.Length > 0) sbPks.Append(", ");
                sbPks.Append("\"").Append(p.name).Append("\"");
            }
            sbPks.Insert(0, "new string[] {");
            sbPks.Append("}");
            foreach (doColumns.uoColumns c in listColumn)
            {
                if (c.column_name.Trim().IndexOf(" ") > 0) c.column_name = c.column_name.Trim().Replace(" ", "_space_");
                string decorateColName = Util.ConvertStyle(c.column_name, cf.DecorateStyle);
                bool isPrimaryKey = false;
                foreach (doPKs.uoPKs p in listPK)
                {
                    if (c.column_name.Equals(p.name))
                    {
                        isPrimaryKey = true;
                        break;
                    }
                }
                StringBuilder sbColumnInfo = new StringBuilder();
                if (isPrimaryKey)
                {
                    sbColumnInfo.Append("Primary Key,");
                }
                sbColumnInfo.Append("Database Type:").Append(c.data_type);
                if (!string.IsNullOrEmpty(c.max_length))
                {
                    sbColumnInfo.Append(",Max Length:").Append(c.max_length);
                }
                sbColumnInfo.Append(",Is Nullable:").Append(c.is_nullable);
                if (!string.IsNullOrEmpty(c.column_default))
                {
                    sbColumnInfo.Append(",Default Value:").Append(c.column_default);
                }
                if (fieldsGeneration != null && fieldsGeneration.ContainsKey(c.column_name))
                {
                    sbColumnInfo.Append(",Generate method:").Append(fieldsGeneration[c.column_name].FieldGenerateMethod);
                    if (fieldsGeneration[c.column_name].FieldGenerateMethodExtraInfo != null)
                    {
                        if (fieldsGeneration[c.column_name].FieldGenerateMethod == DataMapping.FieldGenerateMethod.Native)
                            sbColumnInfo.Append(",Sequence Id:").Append(fieldsGeneration[c.column_name].FieldGenerateMethodExtraInfo);
                        else
                        {
                            sbColumnInfo.Append(",Generate Info:").Append(fieldsGeneration[c.column_name].FieldGenerateMethodExtraInfo);
                        }
                    }
                }
                foreach (doRemarks.uoRemarks r in listRemarks)
                {
                    if (r.column_name.Equals(c.column_name))
                    {
                        sbColumnInfo.Append(",Remark:").Append(r.remark);
                        break;
                    }
                }
                sb_ret.AppendLine("\t\t\t/// <summary>");
                sb_ret.Append("\t\t\t///").AppendLine(sbColumnInfo.ToString());
                sb_ret.AppendLine("\t\t\t/// </summary>");
                sb_ret.Append("\t\t\t").Append(decorateColName).AppendLine(",");
                string vtype = CodeScript.GetColumnType(c.data_type);
                sb_columns.AppendLine(string.Format("\t\t\tprivate {0} _{1};", vtype, decorateColName));
                sb_columns.AppendLine("\t\t\t/// <summary>");
                sb_columns.Append("\t\t\t///").AppendLine(sbColumnInfo.ToString());
                sb_columns.AppendLine("\t\t\t/// </summary>");
                sb_columns.Append(string.Format("\t\t\t[Mapping(\"{0}\"", c.column_name));
                const string specailAttr = ",\"{0}\"";
                if (isPrimaryKey)//when Primary Key is Sequence,not insert
                {
                    sb_columns.AppendFormat(specailAttr, ",un-update");
                }
                else if (cf.UnInsertAndUnUpdate.ToLower().IndexOf(decorateColName.ToLower()) >= 0)
                {
                    sb_columns.AppendFormat(specailAttr, ",un-insert,un-update");
                }
                else if (cf.UnInsert.ToLower().IndexOf(decorateColName.ToLower()) >= 0)
                {
                    sb_columns.AppendFormat(specailAttr, ",un-insert");
                }
                else if (cf.UnUpdate.ToLower().IndexOf(decorateColName.ToLower()) >= 0)
                {
                    sb_columns.AppendFormat(specailAttr, ",un-update");
                }
                if (fieldsGeneration != null && fieldsGeneration.ContainsKey(decorateColName.ToLower()))
                {
                    sb_columns.AppendFormat(", null, DataMapping.FieldGenerateMethod.{0}, \"{1}\"", fieldsGeneration[decorateColName.ToLower()].FieldGenerateMethod, fieldsGeneration[decorateColName.ToLower()].FieldGenerateMethodExtraInfo);
                }
                sb_columns.AppendLine(")]");
                sb_columns.AppendLine(string.Format("\t\t\tpublic {0} {1}", vtype, decorateColName));
                sb_columns.AppendLine("\t\t\t{");
                sb_columns.AppendLine("\t\t\t\tget");
                sb_columns.AppendLine("\t\t\t\t{");
                sb_columns.AppendLine(string.Format("\t\t\t\t\treturn _{0};", decorateColName));
                sb_columns.AppendLine("\t\t\t\t}");
                sb_columns.AppendLine("\t\t\t\tset");
                sb_columns.AppendLine("\t\t\t\t{");
                sb_columns.AppendLine(string.Format("\t\t\t\t\t_{0} = value;", decorateColName));
                sb_columns.AppendLine("\t\t\t\t}");
                sb_columns.AppendLine("\t\t\t}");
            }
            sb_columns.AppendLine("\t\t\t#endregion");
            sb_ret.AppendLine("\t\t}");
            sb_ret.AppendLine(string.Format("\t\tpublic {1}{0}()", decorateTableName, cf.DOPrefix));
            sb_ret.AppendLine("\t\t{");
            if (listPK.Count > 0)
            {
                sb_ret.AppendLine(string.Format("\t\t\tConnInfo = new ConnectionInformation(\"{0}\", {1},{2});", tableName, cf.ConnectionKey, sbPks.ToString()));
            }
            else
            {
                sb_ret.AppendLine(string.Format("\t\t\tConnInfo = new ConnectionInformation(\"{0}\", {1});", tableName, cf.ConnectionKey));
            }
            sb_ret.AppendLine("\t\t}");
            if (!string.IsNullOrEmpty(table.Comments))
            {
                sb_ret.AppendLine("\t\t/// <summary>");
                sb_ret.AppendLine(string.Concat("\t\t///", table.Comments.Replace("\r\n", " ").Replace("\n", " ")));
                sb_ret.AppendLine("\t\t/// </summary>");
            }
            sb_ret.AppendLine(string.Format("\t\tpublic class {1}{0} : UOBase<{1}{0},{2}{0}>", decorateTableName, cf.UOPrefix, cf.UOListPrefix));
            sb_ret.AppendLine("\t\t{");
            sb_ret.AppendLine(sb_columns.ToString());
            sb_ret.AppendLine(string.Format("\t\t\tpublic {1}{0}()", decorateTableName, cf.UOPrefix));
            sb_ret.AppendLine("\t\t\t{");
            sb_ret.AppendLine(string.Format("\t\t\t\tConnInfo = new {0}{1}().ConnInfo;", cf.DOPrefix, decorateTableName));
            sb_ret.AppendLine("\t\t\t}");
            sb_ret.AppendLine("\t\t}");
            sb_ret.AppendLine(string.Format("\t\tpublic class {1}{0} : CommonLibrary.ObjectBase.ListBase<{2}{0}>", decorateTableName, cf.UOListPrefix, cf.UOPrefix));
            sb_ret.AppendLine("\t\t{");
            sb_ret.AppendLine(string.Format("\t\t\tpublic {1}{0}()", decorateTableName, cf.UOListPrefix));
            sb_ret.AppendLine("\t\t\t{");
            sb_ret.AppendLine("\t\t\t}");
            sb_ret.AppendLine("\t\t}");
            sb_ret.AppendLine("\t}");
            sb_ret.AppendLine("}");
            return sb_ret.ToString();
        }

        public static string GetBusinessObject(Settings cf, doTables.uoTables table, doColumns.uoListColumns listColumn, doPKs.uoListPKs listPK)
        {
            string tableName = table.name;
            string decorateTableName = Util.ConvertStyle(tableName, cf.DecorateStyle);
            StringBuilder sb_ret = new StringBuilder();
            StringBuilder sbCondition = new StringBuilder();
            StringBuilder sbParameter = new StringBuilder();
            StringBuilder sbValues = new StringBuilder();
            StringBuilder sbPagingCondition = new StringBuilder();
            sbPagingCondition.AppendLine("\t\t\tParameterCollection objectConditions = new ParameterCollection();");
            sbPagingCondition.AppendLine("\t\t\tTokenTypes tt = tokenTypes;");
            sbPagingCondition.AppendLine("\t\t\tParameterType pt = isAnd ? ParameterType.And : ParameterType.Or;");
            foreach (doColumns.uoColumns c in listColumn)
            {
                if (c.column_name.Trim().IndexOf(" ") > 0) c.column_name = c.column_name.Replace(" ", "_space_");
                string decorateColName = Util.ConvertStyle(c.column_name, cf.DecorateStyle);
                if (GetColumnType(c.data_type).Equals("System.String"))
                {
                    sbPagingCondition.AppendLine(string.Format("\t\t\tif (!string.IsNullOrEmpty(parameterObj.{0}))", decorateColName));
                    sbPagingCondition.AppendLine("\t\t\t{");
                    sbPagingCondition.AppendLine(string.Format("\t\t\t\tobjectConditions.AddCondition(pt, GetColumnTokenType(tt,{0}{1}.Columns.{2},extTokens), {0}{1}.Columns.{2},parameterObj.{2});", cf.DOPrefix, decorateTableName, decorateColName));
                    sbPagingCondition.AppendLine("\t\t\t}");
                }
                else if (GetColumnType(c.data_type).Equals("System.Int32") || GetColumnType(c.data_type).Equals("System.Decimal") || GetColumnType(c.data_type).Equals("System.Single") || GetColumnType(c.data_type).Equals("System.Double"))
                {
                    sbPagingCondition.AppendLine(string.Format("\t\t\tif (parameterObj.{0} != 0 || (extTokens != null && extTokens.ContainsKey({1}{2}.Columns.{0})))", decorateColName, cf.DOPrefix, decorateTableName));
                    sbPagingCondition.AppendLine("\t\t\t{");
                    sbPagingCondition.AppendLine(string.Format("\t\t\t\tobjectConditions.AddCondition(pt, GetColumnTokenType(tt,{0}{1}.Columns.{2},extTokens), {0}{1}.Columns.{2},parameterObj.{2});", cf.DOPrefix, decorateTableName, decorateColName));
                    sbPagingCondition.AppendLine("\t\t\t}");
                }
                else if (GetColumnType(c.data_type).Equals("System.DateTime"))
                {
                    sbPagingCondition.AppendLine(string.Format("\t\t\tif (parameterObj.{0} != DateTime.MinValue)", decorateColName));
                    sbPagingCondition.AppendLine("\t\t\t{");
                    sbPagingCondition.AppendLine(string.Format("\t\t\t\tobjectConditions.AddCondition(pt, GetColumnTokenType(tt,{0}{1}.Columns.{2},extTokens), {0}{1}.Columns.{2},parameterObj.{2});", cf.DOPrefix, decorateTableName, decorateColName));
                    sbPagingCondition.AppendLine("\t\t\t}");
                }
            }
            sbCondition.AppendLine("\t\t\tParameterCollection primaryConditions = new ParameterCollection();");
            for (int i = 0; i < listPK.Count; i++)
            {
                if (listPK[i].name.Trim().IndexOf(" ") > 0) listPK[i].name = listPK[i].name.Trim().Replace(" ", "_space_");
                string decoratePKName = Util.ConvertStyle(listPK[i].name, cf.DecorateStyle);
                string camelPKName = Util.ConvertStyle(listPK[i].name, CodeDecorateStyle.Camel);
                if (sbParameter.Length > 0) sbParameter.Append(",");
                sbParameter.Append(GetColumnType(listColumn, listPK[i].name)).Append(" ").Append(camelPKName);
                if (sbValues.Length > 0) sbValues.Append(",");
                sbValues.Append(camelPKName);
                if (i == 0)
                {
                    sbCondition.AppendLine(string.Format("\t\t\tprimaryConditions.AddCondition(ParameterType.Initial, TokenTypes.Equal, {0}{1}.Columns.{2}, {3});", cf.DOPrefix, decorateTableName, decoratePKName, camelPKName));
                }
                else
                {
                    sbCondition.AppendLine(string.Format("\t\t\tprimaryConditions.AddCondition(ParameterType.And, TokenTypes.Equal, {0}{1}.Columns.{2}, {3});", cf.DOPrefix, decorateTableName, decoratePKName, camelPKName));
                }
            }
            sb_ret.AppendLine("using System;");
            sb_ret.AppendLine("using System.Collections.Generic;");
            sb_ret.AppendLine("using System.Text;");
            sb_ret.AppendLine("using System.Data;");
            sb_ret.AppendLine("using OracleDataAccess;");
            sb_ret.AppendLine("using OracleDataAccess.Data;");
            sb_ret.AppendLine("using DataMapping;");
            sb_ret.Append("using ").Append(cf.DataObjectNameSpace).AppendLine(";");
            sb_ret.AppendLine("");

            sb_ret.Append("namespace ").AppendLine(cf.BusinessObjectNameSpace);
            sb_ret.AppendLine("{");
            if (!string.IsNullOrEmpty(table.Comments))
            {
                sb_ret.AppendLine("\t/// <summary>");
                sb_ret.AppendLine(string.Concat("\t///", "BO Unit:", table.Comments.Replace("\r\n", " ").Replace("\n", " ")));
                sb_ret.AppendLine("\t/// </summary>");
            }
            sb_ret.AppendLine(string.Format("\tpublic class {0}{1}", cf.BOPrefix, decorateTableName));
            sb_ret.AppendLine("\t{");

            sb_ret.Append("\t\t#region This source code was auto-generated by tool,Version=").AppendLine(Config.Version);

            sb_ret.AppendLine(string.Format(@"
                //------------------------------------------------------------------------------
                // <auto-generated>
                //     Date time = {1}
                //     This code was generated by tool,Version={0}.
                //     Changes to this code may cause incorrect behavior and will be lost if
                //     the code is regenerated.
                // </auto-generated>
                //------------------------------------------------------------------------------", Config.Version, DateTime.Now));
            sb_ret.AppendLine("");
            sb_ret.AppendLine("\t\t#region Condition functions");
            if (listPK.Count > 0)
            {
                sb_ret.AppendLine("\t\t///<summary>");
                sb_ret.AppendLine("\t\t///Get conditions by primary key.");
                sb_ret.AppendLine("\t\t///</summary>");
                sb_ret.AppendLine(string.Format("\t\tpublic static ParameterCollection GetConditionsByPrimaryKey({0})", sbParameter.ToString()));
                sb_ret.AppendLine("\t\t{");
                sb_ret.Append(sbCondition.ToString());
                sb_ret.AppendLine("\t\t\treturn primaryConditions;");
                sb_ret.AppendLine("\t\t}");
            }
            sb_ret.AppendLine("");
            sb_ret.AppendLine("\t\t///<summary>");
            sb_ret.AppendLine("\t\t///Get the tokenType of the column of condition query.");
            sb_ret.AppendLine("\t\t///</summary>");
            sb_ret.AppendLine(string.Format("\t\tprivate static TokenTypes GetColumnTokenType(TokenTypes defaultTokenType,{0}{1}.Columns column,Dictionary<{0}{1}.Columns,TokenTypes> extTokens)", cf.DOPrefix, decorateTableName, cf.UOPrefix));
            sb_ret.AppendLine("\t\t{");
            sb_ret.AppendLine("\t\t\tif (extTokens != null && extTokens.ContainsKey(column))");
            sb_ret.AppendLine("\t\t\t\treturn extTokens[column];");
            sb_ret.AppendLine("\t\t\telse");
            sb_ret.AppendLine("\t\t\t\treturn defaultTokenType;");
            sb_ret.AppendLine("\t\t}");
            sb_ret.AppendLine("");
            sb_ret.AppendLine("\t\t///<summary>");
            sb_ret.AppendLine("\t\t///Get conditions by object with Multi-TokenType.");
            sb_ret.AppendLine("\t\t///</summary>");
            sb_ret.AppendLine(string.Format("\t\tpublic static ParameterCollection GetConditionsByObject({0}{1}.{2}{1} parameterObj, bool isAnd, TokenTypes tokenTypes, Dictionary<{0}{1}.Columns, TokenTypes> extTokens)", cf.DOPrefix, decorateTableName, cf.UOPrefix));
            sb_ret.AppendLine("\t\t{");
            sb_ret.Append(sbPagingCondition.ToString());
            sb_ret.AppendLine("\t\t\treturn objectConditions;");
            sb_ret.AppendLine("\t\t}");
            sb_ret.AppendLine("\t\t#endregion");
            sb_ret.AppendLine("");

            sb_ret.AppendLine("\t\t#region Query functions");
            sb_ret.AppendLine("");
            sb_ret.AppendLine("\t\t///<summary>");
            sb_ret.AppendLine("\t\t///Get all records.");
            sb_ret.AppendLine("\t\t///</summary>");
            sb_ret.AppendLine(string.Format("\t\tpublic static {0}{1}.{2}{1} GetAllList({3})", cf.DOPrefix, decorateTableName, cf.UOListPrefix, cf.IsPassConnectionStringToBusiness ? "string connString" : ""));
            sb_ret.AppendLine("\t\t{");
            sb_ret.AppendLine(string.Format("\t\t\t{0}{1} da = new {0}{1}();", cf.DOPrefix, decorateTableName));
            if (cf.IsPassConnectionStringToBusiness)
            {
                sb_ret.AppendLine("\t\t\tda.ConnInfo.ConnectionString = connString;");
            }
            sb_ret.AppendLine("\t\t\treturn da.GetAllList();");
            sb_ret.AppendLine("\t\t}");

            sb_ret.AppendLine("");
            sb_ret.AppendLine("\t\t///<summary>");
            sb_ret.AppendLine("\t\t///Get all records count.");
            sb_ret.AppendLine("\t\t///</summary>");
            sb_ret.AppendLine(string.Format("\t\tpublic static int GetAllRecordsCount({0})", cf.IsPassConnectionStringToBusiness ? "string connString" : ""));
            sb_ret.AppendLine("\t\t{");
            sb_ret.AppendLine(string.Format("\t\t\t{0}{1} da = new {0}{1}();", cf.DOPrefix, decorateTableName));
            if (cf.IsPassConnectionStringToBusiness)
            {
                sb_ret.AppendLine("\t\t\tda.ConnInfo.ConnectionString = connString;");
            }
            sb_ret.AppendLine("\t\t\treturn da.GetRecordsCount();");
            sb_ret.AppendLine("\t\t}");

            sb_ret.AppendLine("");
            sb_ret.AppendLine("\t\t///<summary>");
            sb_ret.AppendLine("\t\t///Get records count.");
            sb_ret.AppendLine("\t\t///</summary>");
            sb_ret.AppendLine(string.Format("\t\tpublic static int GetRecordsCount({3}{0}{1}.{2}{1} parameterObj)", cf.DOPrefix, decorateTableName, cf.UOPrefix, cf.IsPassConnectionStringToBusiness ? "string connString, " : ""));
            sb_ret.AppendLine("\t\t{");
            sb_ret.AppendLine(string.Format("\t\t\treturn GetRecordsCount({0}parameterObj, true, TokenTypes.Equal,null);", cf.IsPassConnectionStringToBusiness ? "connString, " : ""));
            sb_ret.AppendLine("\t\t}");

            sb_ret.AppendLine("");
            sb_ret.AppendLine("\t\t///<summary>");
            sb_ret.AppendLine("\t\t///Get records count.");
            sb_ret.AppendLine("\t\t///</summary>");
            sb_ret.AppendLine(string.Format("\t\tpublic static int GetRecordsCount({3}{0}{1}.{2}{1} parameterObj, bool isAnd, TokenTypes tokenTypes, Dictionary<{0}{1}.Columns, TokenTypes> extTokens)", cf.DOPrefix, decorateTableName, cf.UOPrefix, cf.IsPassConnectionStringToBusiness ? "string connString, " : ""));
            sb_ret.AppendLine("\t\t{");
            sb_ret.AppendLine(string.Format("\t\t\t{0}{1} da = new {0}{1}();", cf.DOPrefix, decorateTableName));
            if (cf.IsPassConnectionStringToBusiness)
            {
                sb_ret.AppendLine("\t\t\tda.ConnInfo.ConnectionString = connString;");
            }
            sb_ret.AppendLine("\t\t\treturn da.GetRecordsCount(GetConditionsByObject(parameterObj, isAnd, tokenTypes, extTokens));");
            sb_ret.AppendLine("\t\t}");

            sb_ret.AppendLine("");
            sb_ret.AppendLine("\t\t///<summary>");
            sb_ret.AppendLine("\t\t///Get list by object.");
            sb_ret.AppendLine("\t\t///</summary>");
            sb_ret.AppendLine(string.Format("\t\tpublic static {0}{1}.{2}{1} GetList({4}{0}{1}.{3}{1} parameterObj, bool isAnd, TokenTypes tokenTypes, Dictionary<{0}{1}.Columns, TokenTypes> extTokens)", cf.DOPrefix, decorateTableName, cf.UOListPrefix, cf.UOPrefix, cf.IsPassConnectionStringToBusiness ? "string connString, " : ""));
            sb_ret.AppendLine("\t\t{");
            if (cf.IsPassConnectionStringToBusiness)
            {
                sb_ret.AppendLine("\t\t\tparameterObj.ConnInfo.ConnectionString = connString;");
            }
            sb_ret.AppendLine("\t\t\treturn parameterObj.GetList(GetConditionsByObject(parameterObj, isAnd, tokenTypes, extTokens));");
            sb_ret.AppendLine("\t\t}");

            sb_ret.AppendLine("");
            sb_ret.AppendLine("\t\t///<summary>");
            sb_ret.AppendLine("\t\t///Get list by object.");
            sb_ret.AppendLine("\t\t///</summary>");
            sb_ret.AppendLine(string.Format("\t\tpublic static {0}{1}.{2}{1} GetList({4}{0}{1}.{3}{1} parameterObj)", cf.DOPrefix, decorateTableName, cf.UOListPrefix, cf.UOPrefix, cf.IsPassConnectionStringToBusiness ? "string connString, " : ""));
            sb_ret.AppendLine("\t\t{");
            if (cf.IsPassConnectionStringToBusiness)
            {
                sb_ret.AppendLine("\t\t\tparameterObj.ConnInfo.ConnectionString = connString;");
            }
            sb_ret.AppendLine(string.Format("\t\t\treturn GetList({0}parameterObj, true, TokenTypes.Equal, null);", cf.IsPassConnectionStringToBusiness ? "connString, " : ""));
            sb_ret.AppendLine("\t\t}");
            if (listPK.Count > 0)
            {
                sb_ret.AppendLine("");
                sb_ret.AppendLine("\t\t///<summary>");
                sb_ret.AppendLine("\t\t///Get object by primary key.");
                sb_ret.AppendLine("\t\t///</summary>");
                sb_ret.AppendLine(string.Format("\t\tpublic static {0}{1}.{2}{1} GetObject({4}{3})", cf.DOPrefix, decorateTableName, cf.UOPrefix, sbParameter.ToString(), cf.IsPassConnectionStringToBusiness ? "string connString, " : ""));
                sb_ret.AppendLine("\t\t{");
                sb_ret.AppendLine(string.Format("\t\t\t{0}{1} da = new {0}{1}();", cf.DOPrefix, decorateTableName));
                if (cf.IsPassConnectionStringToBusiness)
                {
                    sb_ret.AppendLine("\t\t\tda.ConnInfo.ConnectionString = connString;");
                }
                sb_ret.AppendLine(string.Format("\t\t\t{0}{1}.{2}{1} l = da.GetList(GetConditionsByPrimaryKey({3}));", cf.DOPrefix, decorateTableName, cf.UOListPrefix, sbValues.ToString()));
                sb_ret.AppendLine("\t\t\treturn l.Count > 0 ? l[0] : null;");
                sb_ret.AppendLine("\t\t}");
            }
            sb_ret.AppendLine("");
            sb_ret.AppendLine("\t\t///<summary>");
            sb_ret.AppendLine("\t\t///Get paging list.");
            sb_ret.AppendLine("\t\t///</summary>");
            sb_ret.AppendLine(string.Format("\t\tpublic static PagingResult<{0}{1}.{3}{1}, {0}{1}.{2}{1}> GetPagingList({4}{0}{1}.{3}{1} parameterObj,int pageNumber, int pageSize,string sortBy,bool isAsc, bool isAnd, TokenTypes tokenTypes, Dictionary<{0}{1}.Columns, TokenTypes> extTokens)", cf.DOPrefix, decorateTableName, cf.UOListPrefix, cf.UOPrefix, cf.IsPassConnectionStringToBusiness ? "string connString, " : ""));
            sb_ret.AppendLine("\t\t{");
            if (cf.IsPassConnectionStringToBusiness)
            {
                sb_ret.AppendLine("\t\t\tparameterObj.ConnInfo.ConnectionString = connString;");
            }
            sb_ret.AppendLine(string.Format("\t\t\treturn parameterObj.GetPagingList(GetConditionsByObject(parameterObj, isAnd, tokenTypes,extTokens), pageNumber, pageSize, sortBy, isAsc);", cf.DOPrefix, decorateTableName));
            sb_ret.AppendLine("\t\t}");

            sb_ret.AppendLine("");
            sb_ret.AppendLine("\t\t///<summary>");
            sb_ret.AppendLine("\t\t///Get paging list.");
            sb_ret.AppendLine("\t\t///</summary>");
            sb_ret.AppendLine(string.Format("\t\tpublic static PagingResult<{0}{1}.{3}{1}, {0}{1}.{2}{1}> GetPagingList({4}{0}{1}.{3}{1} parameterObj,int pageNumber, int pageSize,string sortBy,bool isAsc)", cf.DOPrefix, decorateTableName, cf.UOListPrefix, cf.UOPrefix, cf.IsPassConnectionStringToBusiness ? "string connString, " : ""));
            sb_ret.AppendLine("\t\t{");
            if (cf.IsPassConnectionStringToBusiness)
            {
                sb_ret.AppendLine("\t\t\tparameterObj.ConnInfo.ConnectionString = connString;");
            }
            sb_ret.AppendLine(string.Format("\t\t\treturn parameterObj.GetPagingList(GetConditionsByObject(parameterObj, true, TokenTypes.Like,null), pageNumber, pageSize, sortBy, isAsc);", cf.IsPassConnectionStringToBusiness ? "string connString" : ""));
            sb_ret.AppendLine("\t\t}");

            sb_ret.AppendLine("\t\t#endregion");
            sb_ret.AppendLine("");
            if (listPK.Count > 0)
            {
                sb_ret.AppendLine("\t\t#region Update functions");
                sb_ret.AppendLine("\t\t///<summary>");
                sb_ret.AppendLine("\t\t///Update object by primary key.");
                sb_ret.AppendLine("\t\t///</summary>");
                sb_ret.AppendLine(string.Format("\t\tpublic static bool UpdateObject({4}{0}{1}.{2}{1} obj, {3})", cf.DOPrefix, decorateTableName, cf.UOPrefix, sbParameter.ToString(), cf.IsPassConnectionStringToBusiness ? "string connString, " : ""));
                sb_ret.AppendLine("\t\t{");
                if (cf.IsPassConnectionStringToBusiness)
                {
                    sb_ret.AppendLine("\t\t\tobj.ConnInfo.ConnectionString = connString;");
                }
                sb_ret.AppendLine(string.Format("\t\t\treturn obj.Update(GetConditionsByPrimaryKey({0})) > 0;", sbValues.ToString()));
                sb_ret.AppendLine("\t\t}");

                sb_ret.AppendLine("");
                sb_ret.AppendLine("\t\t///<summary>");
                sb_ret.AppendLine("\t\t///Update object by primary key(with transation).");
                sb_ret.AppendLine("\t\t///</summary>");
                sb_ret.AppendLine(string.Format("\t\tpublic static bool UpdateObject({4}{0}{1}.{2}{1} obj, {3}, IDbConnection connection, IDbTransaction transaction)", cf.DOPrefix, decorateTableName, cf.UOPrefix, sbParameter.ToString(), cf.IsPassConnectionStringToBusiness ? "string connString, " : ""));
                sb_ret.AppendLine("\t\t{");
                if (cf.IsPassConnectionStringToBusiness)
                {
                    sb_ret.AppendLine("\t\t\tobj.ConnInfo.ConnectionString = connString;");
                }
                sb_ret.AppendLine(string.Format("\t\t\treturn obj.Update(connection, transaction, GetConditionsByPrimaryKey({0})) > 0;", sbValues.ToString()));
                sb_ret.AppendLine("\t\t}");
                sb_ret.AppendLine("\t\t#endregion");

                sb_ret.AppendLine("");
                sb_ret.AppendLine("\t\t#region Delete functions");
                sb_ret.AppendLine("\t\t///<summary>");
                sb_ret.AppendLine("\t\t///Delete object by primary key.");
                sb_ret.AppendLine("\t\t///</summary>");
                sb_ret.AppendLine(string.Format("\t\tpublic static int Delete({1}{0})", sbParameter.ToString(), cf.IsPassConnectionStringToBusiness ? "string connString, " : ""));
                sb_ret.AppendLine("\t\t{");
                sb_ret.AppendLine(string.Format("\t\t\t{0}{1} da = new {0}{1}();", cf.DOPrefix, decorateTableName));
                if (cf.IsPassConnectionStringToBusiness)
                {
                    sb_ret.AppendLine("\t\t\tda.ConnInfo.ConnectionString = connString;");
                }
                sb_ret.AppendLine(string.Format("\t\t\treturn da.Delete(GetConditionsByPrimaryKey({0}));", sbValues.ToString()));
                sb_ret.AppendLine("\t\t}");

                sb_ret.AppendLine("");
                sb_ret.AppendLine("\t\t///<summary>");
                sb_ret.AppendLine("\t\t///Delete object by primary key(with transation).");
                sb_ret.AppendLine("\t\t///</summary>");
                sb_ret.AppendLine(string.Format("\t\tpublic static int Delete({1}{0}, IDbConnection connection, IDbTransaction transaction)", sbParameter.ToString(), cf.IsPassConnectionStringToBusiness ? "string connString, " : ""));
                sb_ret.AppendLine("\t\t{");
                sb_ret.AppendLine(string.Format("\t\t\t{0}{1} da = new {0}{1}();", cf.DOPrefix, decorateTableName));
                if (cf.IsPassConnectionStringToBusiness)
                {
                    sb_ret.AppendLine("\t\t\tda.ConnInfo.ConnectionString = connString;");
                }
                sb_ret.AppendLine(string.Format("\t\t\treturn da.Delete(connection, transaction, GetConditionsByPrimaryKey({0}));", sbValues.ToString()));
                sb_ret.AppendLine("\t\t}");
                sb_ret.AppendLine("\t\t#endregion");
                sb_ret.AppendLine("");
            }
            sb_ret.AppendLine("\t\t#endregion");

            sb_ret.AppendLine("");
            sb_ret.AppendLine("\t\t#region User extensions");
            sb_ret.AppendLine("");
            sb_ret.AppendLine("\t\t#endregion");

            sb_ret.AppendLine("\t}");
            sb_ret.AppendLine("}");
            return sb_ret.ToString();
        }

        #endregion
    }
}
