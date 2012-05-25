/* 
 * CSV文件反射成对应的类
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

namespace CommonLibrary.CSV
{
    /// <summary>
    /// CSV格式对象加载器
    /// </summary>
    public class ObjectLoader
    {
        /// <summary>
        /// 将对应的字段值加载到对象中
        /// </summary>
        /// <typeparam name="X">目标类型</typeparam>
        /// <param name="fields">字段值</param>
        /// <param name="isSupressErrors">是否忽略错误</param>
        /// <returns>目标对象</returns>
        public static X LoadNew<X>(string[] fields, bool isSupressErrors)
        {
            X tempObj = (X)Activator.CreateInstance(typeof(X));
            Load(tempObj, fields, isSupressErrors);
            return tempObj;
        }

        /// <summary>
        /// 将对应的字段值加载到对象中
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="fields">字段值</param>
        /// <param name="isSupressErrors">>是否忽略错误</param>
        public static void Load(object target, string[] fields, bool isSupressErrors)
        {
            Type targetType = target.GetType();
            PropertyInfo[] properties = targetType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.CanWrite)
                {
                    object[] attributes = property.GetCustomAttributes(typeof(CSVAttribute), false);
                    if (attributes.Length > 0)
                    {
                        CSVAttribute positionAttr = (CSVAttribute)attributes[0];
                        int position = positionAttr.Position;
                        try
                        {
                            object data = fields[position];
                            if (positionAttr.DataTransform != string.Empty)
                            {
                                MethodInfo method = targetType.GetMethod(positionAttr.DataTransform);

                                data = method.Invoke(target, new object[] { data });
                            }
                            property.SetValue(target, Convert.ChangeType(data, property.PropertyType), null);
                        }
                        catch
                        {
                            if (!isSupressErrors)
                                throw;
                        }

                    }
                }
            }
        }
    }
}
