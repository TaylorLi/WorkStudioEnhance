using System;
using System.Collections.Generic;
using System.Text;
using DataMapping;

namespace OracleCodeBuilderLibrary.DataObjects
{
    public class doPKs : OracleDataAccess.Data.DOBase<doPKs.uoPKs, doPKs.uoListPKs>
    {
        public doPKs()
        {

        }
        public enum Columns
        {
            name
        }
        public class uoPKs : OracleDataAccess.Data.UOBase<uoPKs, uoListPKs>
        {
            public uoPKs()
            {

            }
            private string _name;
            [Mapping("COLUMN_NAME")]
            public string name
            {
                get { return _name; }
                set
                {
                    _name = value;
                }
            }
        }
        public class uoListPKs : CommonLibrary.ObjectBase.ListBase<uoPKs>
        {
            public uoListPKs()
            {

            }
        }
    }
}
