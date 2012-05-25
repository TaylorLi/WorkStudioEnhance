using System;
using System.Collections.Generic;
using System.Text;
using DataMapping;

namespace OracleCodeBuilderLibrary.DataObjects
{
    public class doRemarks : OracleDataAccess.Data.DOBase<doRemarks.uoRemarks, doRemarks.uoListRemarks>
    {
        public doRemarks()
        {

        }
        public doRemarks(string connectionKey)
        {

        }
        public class uoRemarks : OracleDataAccess.Data.UOBase<uoRemarks, uoListRemarks>
        {
            public uoRemarks()
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
            private string _remark;
            [Mapping("COMMENTS")]
            public string remark
            {
                get { return _remark; }
                set { _remark = value; }
            }
        }
        public class uoListRemarks : CommonLibrary.ObjectBase.ListBase<uoRemarks>
        {
            public uoListRemarks()
            {

            }
        }
    }
}
