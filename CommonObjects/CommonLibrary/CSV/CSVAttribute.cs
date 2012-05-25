/* 
 *属性与CSV文件列位置的对应自定义属性
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

namespace CommonLibrary.CSV
{
    public delegate object DataTranformDelegate(string data);
    /// <summary>
    /// 属性与CSV文件列位置的对应
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CSVAttribute : System.Attribute
    {
        public int Position;
        public string DataTransform = string.Empty;

        /// <summary>
        /// 标识与CVS列的对应关系
        /// </summary>
        /// <param name="position">对应列位置</param>
        /// <param name="dataTransform">对该列进行转换的函数的完全名称</param>
        public CSVAttribute(int position, string dataTransform)
        {
            Position = position;
            DataTransform = dataTransform;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position">对应列位置</param>
        public CSVAttribute(int position)
        {
            Position = position;
        }

        public CSVAttribute()
        {
        }

    }
}
