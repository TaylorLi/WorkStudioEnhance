/* 
 *序列化基类，包括Xml的序列化
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
using System.IO;
using System.Xml;

namespace CommonLibrary.ObjectBase
{
    public class SerializationBase<T> where T : class, new()
    {
        public string ToXml()
        {
            return Utility.SerializationHelper.SerializeToXml(this);
        }

        public T FormXml(string xml)
        {
            return Utility.SerializationHelper.FromXml<T>(xml);
        }
    }
}
