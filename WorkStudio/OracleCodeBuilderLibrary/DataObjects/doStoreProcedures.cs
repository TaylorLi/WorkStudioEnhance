using System;
using System.Collections.Generic;
using System.Text;
using DataMapping;

namespace OracleCodeBuilderLibrary.DataObjects
{
    public class doStoreProcedures : OracleDataAccess.Data.DOBase<doStoreProcedures.uoStoreProcedures, doStoreProcedures.uoListStoreProcedures>
    {
        public doStoreProcedures()
        {

        }
        public class uoStoreProcedures : OracleDataAccess.Data.UOBase<uoStoreProcedures, uoListStoreProcedures>
        {
            public uoStoreProcedures()
            {

            }
            private string _name;
            [Mapping("OBJECT_NAME")]
            public string name
            {
                get { return _name; }
                set { _name = value; }
            }
        }
        public class uoListStoreProcedures : CommonLibrary.ObjectBase.ListBase<uoStoreProcedures>
        {
            public uoListStoreProcedures()
            {

            }
        }
    }
}
