using System;
using System.Collections.Generic;
using System.Text;
using OracleDataAccess.Data;
using DataMapping;

namespace OracleCodeBuilderLibrary.DataObjects
{
    public class doStoreProcedureParameters : DOBase<doStoreProcedureParameters.uoStoreProcedureParameters, doStoreProcedureParameters.uoListStoreProcedureParameters>
    {
        public enum Columns
        {
            name,
            data_type,
            length,
            isnullable,
            isoutparam,
        }
        public doStoreProcedureParameters()
        {
            ConnInfo = new ConnectionInformation("SELECT tua.ARGUMENT_NAME,tua.DATA_TYPE,tua.POSITION,tua.SEQUENCE,tua.DEFAULT_VALUE,tua.in_out from USER_ARGUMENTS tua where tua.OBJECT_NAME='{0}' order by tua.SEQUENCE", "Adholidays", new string[] { "ARGUMENT_NAME" });
            ConnInfo.IsSqlSentence = true;
        }
        public class uoStoreProcedureParameters : UOBase<uoStoreProcedureParameters, uoListStoreProcedureParameters>
        {
            #region Columns
            private System.String _name;
            [Mapping("ARGUMENT_NAME")]
            public System.String name
            {
                get
                {
                    return _name;
                }
                set
                {
                    _name = value;
                }
            }
            private System.String _data_type;
            [Mapping("DATA_TYPE")]
            public System.String data_type
            {
                get
                {
                    return _data_type;
                }
                set
                {
                    _data_type = value;
                }
            }
            private System.Object _DEFAULT_VALUE;
            [Mapping("DEFAULT_VALUE")]
            public System.Object DEFAULT_VALUE
            {
                get
                {
                    return _DEFAULT_VALUE;
                }
                set
                {
                    _DEFAULT_VALUE = value;
                }
            }            
            private string _IN_OUT;
            [Mapping("IN_OUT")]
            public System.String IN_OUT
            {
                get
                {
                    return _IN_OUT;
                }
                set
                {
                    _IN_OUT = value;
                }
            }
            #endregion

            public uoStoreProcedureParameters()
            {
                ConnInfo = new doStoreProcedureParameters().ConnInfo;
            }
        }
        public class uoListStoreProcedureParameters : CommonLibrary.ObjectBase.ListBase<uoStoreProcedureParameters>
        {
            public uoListStoreProcedureParameters()
            {
            }
        }
    }
}
