/* 
 *������CSV�ļ���λ�õĶ�Ӧ�Զ�������
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

namespace CommonLibrary.CSV
{
    public delegate object DataTranformDelegate(string data);
    /// <summary>
    /// ������CSV�ļ���λ�õĶ�Ӧ
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CSVAttribute : System.Attribute
    {
        public int Position;
        public string DataTransform = string.Empty;

        /// <summary>
        /// ��ʶ��CVS�еĶ�Ӧ��ϵ
        /// </summary>
        /// <param name="position">��Ӧ��λ��</param>
        /// <param name="dataTransform">�Ը��н���ת���ĺ�������ȫ����</param>
        public CSVAttribute(int position, string dataTransform)
        {
            Position = position;
            DataTransform = dataTransform;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position">��Ӧ��λ��</param>
        public CSVAttribute(int position)
        {
            Position = position;
        }

        public CSVAttribute()
        {
        }

    }
}
