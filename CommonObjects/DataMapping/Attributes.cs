using System;

namespace DataMapping
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MappingAttribute : System.Attribute
    {
        public MappingAttribute(string dataFieldName, string specialAttributes, object defaultValue, FieldGenerateMethod generateMethod, object generateMethodExtraInfo)
            : base()
        {
            _DataFieldName = dataFieldName;
            _DefaultValue = defaultValue;
            _SpecialAttributes = specialAttributes;
            _FieldGenerateMethod = generateMethod;
            _FieldGenerateMethodExtraInfo = generateMethodExtraInfo;
        }

        public MappingAttribute(string dataFieldName, string specialAttributes, object defaultValue)
        {
            _DataFieldName = dataFieldName;
            _DefaultValue = defaultValue;
            _SpecialAttributes = specialAttributes;
        }

        public MappingAttribute(string dataFieldName, string specialAttrs)
            : this(dataFieldName, specialAttrs, null) { }

        public MappingAttribute(string dataFieldName)
            : this(dataFieldName != null ? dataFieldName.ToString() : "", null, null) { }


        #region Attributes
        private string _DataFieldName;
        public string DataFieldName
        {
            get { return _DataFieldName; }
        }
        private object _DefaultValue;
        public object DefaultValue
        {
            get { return _DefaultValue; }
        }

        private string _SpecialAttributes;
        public string SpecialAttributes
        {
            get { return _SpecialAttributes; }
            set { _SpecialAttributes = value; }
        }

        private FieldGenerateMethod _FieldGenerateMethod = FieldGenerateMethod.Assigned;

        public FieldGenerateMethod FieldGenerateMethod
        {
            get { return _FieldGenerateMethod; }
            set { _FieldGenerateMethod = value; }
        }

        private object _FieldGenerateMethodExtraInfo;

        public object FieldGenerateMethodExtraInfo
        {
            get { return _FieldGenerateMethodExtraInfo; }
            set { _FieldGenerateMethodExtraInfo = value; }
        }

        #endregion
    }

    public class FieldGenerateInfo
    {
        #region Property

        private FieldGenerateMethod _FieldGenerateMethod = FieldGenerateMethod.Assigned;

        public FieldGenerateMethod FieldGenerateMethod
        {
            get { return _FieldGenerateMethod; }
            set { _FieldGenerateMethod = value; }
        }

        private object _FieldGenerateMethodExtraInfo;

        public object FieldGenerateMethodExtraInfo
        {
            get { return _FieldGenerateMethodExtraInfo; }
            set { _FieldGenerateMethodExtraInfo = value; }
        }
        #endregion

        #region Construction

        public FieldGenerateInfo()
        {

        }

        public FieldGenerateInfo(FieldGenerateMethod generateMethod, object generateMethodExtraInfo)
        {
            _FieldGenerateMethod = generateMethod;
            _FieldGenerateMethodExtraInfo = generateMethodExtraInfo;
        }

        #endregion
    }

    public enum FieldGenerateMethod
    {
        Assigned = 0,
        Native = 1,
    }
}
