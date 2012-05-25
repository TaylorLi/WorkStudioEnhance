using System;
using System.Collections.Generic;
using System.Text;
using OracleDataAccess;
using System.Data;

namespace OracleDataAccess.Data
{
    public class UOListBaseCollection<T> : List<T>
        where T : class, new()
    {
        #region Attributes
        private ConnectionInformation _ConnInfo;
        public ConnectionInformation ConnInfo
        {
            get { return _ConnInfo; }
            set { _ConnInfo = value; }
        }
        #endregion

        #region Constructors
        public UOListBaseCollection()
        {

        }
        #endregion

    }
}
