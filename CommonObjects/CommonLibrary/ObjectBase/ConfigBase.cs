/* 
 *读取Web.config的基类
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
