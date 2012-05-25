using System;
using System.Collections.Generic;
using System.Text;
using DataMapping;

namespace OracleCodeBuilderLibrary.DataObjects
{
    public class doColumns : OracleDataAccess.Data.DOBase<doColumns.uoColumns, doColumns.uoListColumns>
    {
        public doColumns()
        {

        }
        public doColumns(string connectionKey)
        {

        }
        public class uoColumns : OracleDataAccess.Data.UOBase<uoColumns, uoListColumns>
        {
            public uoColumns()
            {

            }
            private string _column_name;
            [Mapping("COLUMN_NAME")]
            public string column_name
            {
                get { return _column_name; }
                set
                {
                    _column_name = value;                    
                }
            }
            private string _column_default;
            [Mapping("DATA_DEFAULT")]
            public string column_default
            {
                get { return _column_default; }
                set { _column_default = value; }
            }
            private string _is_nullable;
            [Mapping("NULLABLE")]
            public string is_nullable
            {
                get { return _is_nullable; }
                set { _is_nullable = value; }
            }
            private string _data_type;
            [Mapping("DATA_TYPE")]
            public string data_type
            {
                get { return _data_type; }
                set { _data_type = value; }
            }
            private string _max_length;
            [Mapping("DATA_LENGTH")]
            public string max_length
            {
                get { return _max_length; }
                set { _max_length = value; }
            }
        }
        public class uoListColumns : CommonLibrary.ObjectBase.ListBase<uoColumns>
        {
            public uoListColumns()
            {

            }
        }
    }
}
