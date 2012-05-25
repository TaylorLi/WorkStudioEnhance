using System;
using System.Collections.Generic;
using System.Text;
using DataMapping;

namespace OracleCodeBuilderLibrary.DataObjects
{
    public class doTables : OracleDataAccess.Data.DOBase<doTables.uoTables, doTables.uoListTables>
    {
        public doTables()
        {

        }
        public class uoTables : OracleDataAccess.Data.UOBase<uoTables, uoListTables>
        {
            public uoTables()
            {

            }
            private string _name;
            [Mapping("TABLE_NAME")]
            public string name
            {
                get { return _name; }
                set { _name = value; }
            }

            private string _TableType;
            [Mapping("TABLE_TYPE")]
            public string TableType
            {
                get { return _TableType; }
                set { _TableType = value; }
            }

            private string _Comments;
            [Mapping("COMMENTS")]
            public string Comments
            {
                get { return _Comments; }
                set { _Comments = value; }
            }


        }
        public class uoListTables : CommonLibrary.ObjectBase.ListBase<uoTables>
        {
            public uoListTables()
            {

            }
        }
    }
}
