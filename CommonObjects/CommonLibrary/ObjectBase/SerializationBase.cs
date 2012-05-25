/* 
 *���л����࣬����Xml�����л�
 *  
 * �������ż���   
 * ʱ�䣺2011��9��5��
 * 
 * �޸ģ�[����]         
 * ʱ�䣺[ʱ��]             
 * ˵����[˵��]
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
