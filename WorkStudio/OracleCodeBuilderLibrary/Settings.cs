using System;
using System.Collections.Generic;
using System.Text;

namespace OracleCodeBuilderLibrary
{
    public class Settings : CommonLibrary.ObjectBase.SerializationBase<Settings>
    {
        private string _ProjectName;

        public string ProjectName
        {
            get { return _ProjectName; }
            set { _ProjectName = value; }
        }

        private string _ConnectionKey;

        public string ConnectionKey
        {
            get { return _ConnectionKey; }
            set { _ConnectionKey = value; }
        }
        private string _DataObjectNameSpace;

        public string DataObjectNameSpace
        {
            get { return _DataObjectNameSpace; }
            set { _DataObjectNameSpace = value; }
        }
        private string _BusinessObjectNameSpace;

        public string BusinessObjectNameSpace
        {
            get { return _BusinessObjectNameSpace; }
            set { _BusinessObjectNameSpace = value; }
        }

        private string _ConnectionString;

        public string ConnectionString
        {
            get { return _ConnectionString; }
            set { _ConnectionString = value; }
        }

        private string _UnInsertAndUnUpdate;

        public string UnInsertAndUnUpdate
        {
            get { return _UnInsertAndUnUpdate; }
            set { _UnInsertAndUnUpdate = value; }
        }
        private string _UnInsert;

        public string UnInsert
        {
            get { return _UnInsert; }
            set { _UnInsert = value; }
        }
        private string _UnUpdate;

        public string UnUpdate
        {
            get { return _UnUpdate; }
            set { _UnUpdate = value; }
        }
        //private string _DataObjectFolder;

        //public string DataObjectFolder
        //{
        //    get { return _DataObjectFolder; }
        //    set { _DataObjectFolder = value; }
        //}
        //private string _BusinessObjectFolder;

        //public string BusinessObjectFolder
        //{
        //    get { return _BusinessObjectFolder; }
        //    set { _BusinessObjectFolder = value; }
        //}

        //private string _UserTable = "Login";

        //public string UserTable
        //{
        //    get { return _UserTable; }
        //    set { _UserTable = value; }
        //}
        //private string _UserPrimaryKeys = "CompanyCode,UserCode";

        //public string UserPrimaryKeys
        //{
        //    get { return _UserPrimaryKeys; }
        //    set { _UserPrimaryKeys = value; }
        //}

        private string _DOPrefix;

        public string DOPrefix
        {
            get { return _DOPrefix; }
            set { _DOPrefix = value; }
        }
        private string _UOPrefix;

        public string UOPrefix
        {
            get { return _UOPrefix; }
            set { _UOPrefix = value; }
        }
        private string _UOListPrefix;

        public string UOListPrefix
        {
            get { return _UOListPrefix; }
            set { _UOListPrefix = value; }
        }
        private string _BOPrefix;

        public string BOPrefix
        {
            get { return _BOPrefix; }
            set { _BOPrefix = value; }
        }
        private bool _IsPassConnectionStringToBusiness;

        public bool IsPassConnectionStringToBusiness
        {
            get { return _IsPassConnectionStringToBusiness; }
            set { _IsPassConnectionStringToBusiness = value; }
        }

        private CodeDecorateStyle _DecorateStyle= CodeDecorateStyle.Pascal;

        public CodeDecorateStyle DecorateStyle
        {
            get { return _DecorateStyle; }
            set { _DecorateStyle = value; }
        }

        public Settings()
        {
            _ProjectName = "HTISP";
            _ConnectionString = "data source=CRM_DEV_76;persist security info=True;user id=insurance;password=insurance;min pool size=4;max pool size=128";
            _ConnectionKey = "Config.ConnectionKeys.HTISP";
            _BOPrefix = "BO_";
            _UOListPrefix = "UOList_";
            _DOPrefix = "DO_";
            _UOPrefix = "UO_";
            _BusinessObjectNameSpace = "HTISP.DAL.BusinessObject";
            _DataObjectNameSpace = "HTISP.DAL.DataObject";
            _DecorateStyle = CodeDecorateStyle.Pascal;

            _UnInsert = "UpdateOn,UpdateBy";
            _UnUpdate = "CreateBy";
            _UnInsertAndUnUpdate = "CreateOn";
        }
    }

    public enum CodeDecorateStyle
    {
        None,
        Camel,
        Pascal,
    }
}
