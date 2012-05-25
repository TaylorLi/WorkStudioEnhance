/* 
 *��ȡWeb.config�Ļ���
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

namespace CommonLibrary.ObjectBase
{
    public class ConfigBase<T> where T : class, new()
    {
        public ConfigBase()
        {

        }

        public virtual void ReadSetting()
        {

        }

        public virtual void InitSetting()
        {

        }

        public virtual void SaveSetting()
        {

        }
    }
}
