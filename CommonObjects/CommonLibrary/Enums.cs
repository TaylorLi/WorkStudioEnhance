/* 
 * 定义常用枚举类型
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

namespace CommonLibrary
{
    public class Enums
    {
        public enum SystemMode
        {
            UNDEFINE = -1,
            DEV = 0,
            UAT = 1,
            LIVE = 2,
        }

        public enum FieldRight
        {
            NoRight = 0,
            ViewOnly = 1,
            Modify = 2,
        }
    }
}
