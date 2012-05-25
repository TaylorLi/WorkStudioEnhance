/* 
 * 自定义属性，用来保存与数据库的对应关系，同时减少多次反射
 *  
 * 创建：杜吉利   
 * 时间：2011年9月5日
 * 
 * 修改：[姓名]         
 * 时间：[时间]             
 * 说明：[说明]
 * 
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace DataMapping
{
    public class PropertyMappingInfo
    {
        #region Constructors
        internal PropertyMappingInfo() : this(string.Empty, null, null, string.Empty) { }

        internal PropertyMappingInfo(string dataFieldName, object defaultValue, PropertyInfo info, string specailAttributes, FieldGenerateInfo fieldGenInfo)
        {
            _dataFieldName = dataFieldName;
            _defaultValue = defaultValue;
            _propInfo = info;
            _SpecialAttributes = specailAttributes;
            _FieldGenInfo = fieldGenInfo;
        }

        internal PropertyMappingInfo(string dataFieldName, object defaultValue, PropertyInfo info, string specailAttributes)
        {
            _dataFieldName = dataFieldName;
            _defaultValue = defaultValue;
            _propInfo = info;
            _SpecialAttributes = specailAttributes;
            _FieldGenInfo = new FieldGenerateInfo(FieldGenerateMethod.Assigned, null);
        }
        #endregion

        #region Public Properties
        private string _dataFieldName;
        public string DataFieldName
        {
            get
            {
                if (string.IsNullOrEmpty(_dataFieldName) && _propInfo != null)
                {
                    _dataFieldName = _propInfo.Name;
                }
                return _dataFieldName;
            }
        }
        private object _defaultValue;
        public object DefaultValue
        {
            get { return _defaultValue; }
        }
        private PropertyInfo _propInfo;
        public PropertyInfo PropertyInfo
        {
            get { return _propInfo; }
        }

        private string _SpecialAttributes;

        public string SpecialAttributes
        {
            get { return _SpecialAttributes; }
            set { _SpecialAttributes = value; }
        }

        private FieldGenerateInfo _FieldGenInfo = new FieldGenerateInfo();

        public FieldGenerateInfo FieldGenInfo
        {
            get { return _FieldGenInfo; }
            set { _FieldGenInfo = value; }
        }

        #endregion
    }
}
