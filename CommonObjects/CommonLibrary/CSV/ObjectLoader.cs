/* 
 * CSV�ļ�����ɶ�Ӧ����
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
using System.Reflection;

namespace CommonLibrary.CSV
{
    /// <summary>
    /// CSV��ʽ���������
    /// </summary>
    public class ObjectLoader
    {
        /// <summary>
        /// ����Ӧ���ֶ�ֵ���ص�������
        /// </summary>
        /// <typeparam name="X">Ŀ������</typeparam>
        /// <param name="fields">�ֶ�ֵ</param>
        /// <param name="isSupressErrors">�Ƿ���Դ���</param>
        /// <returns>Ŀ�����</returns>
        public static X LoadNew<X>(string[] fields, bool isSupressErrors)
        {
            X tempObj = (X)Activator.CreateInstance(typeof(X));
            Load(tempObj, fields, isSupressErrors);
            return tempObj;
        }

        /// <summary>
        /// ����Ӧ���ֶ�ֵ���ص�������
        /// </summary>
        /// <param name="target">Ŀ�����</param>
        /// <param name="fields">�ֶ�ֵ</param>
        /// <param name="isSupressErrors">>�Ƿ���Դ���</param>
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
