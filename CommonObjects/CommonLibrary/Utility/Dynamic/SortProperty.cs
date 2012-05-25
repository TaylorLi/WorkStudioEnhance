/* 
 * 排序属性
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

namespace CommonLibrary.Utility.Dynamic
{
    public struct SortProperty
    {
        #region Properties

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (value == null || value.Trim().Length == 0)
                    throw new ArgumentException("A property cannot have an empty name.");

                name = value.Trim();
            }
        }

        private bool descending;
        public bool Descending
        {
            get { return descending; }
            set { descending = value; }
        }

        #endregion

        #region Constructors

        public SortProperty(string propertyName)
        {
            if (propertyName == null || propertyName.Trim().Length == 0)
                throw new ArgumentException("A property cannot have an empty name.");

            this.name = propertyName.Trim();
            this.descending = false;
        }

        public SortProperty(string propertyName, bool sortDescending)
        {
            if (propertyName == null || propertyName.Trim().Length == 0)
                throw new ArgumentException("A property cannot have an empty name.");

            this.name = propertyName.Trim();
            this.descending = sortDescending;
        }

        #endregion
    }
}
