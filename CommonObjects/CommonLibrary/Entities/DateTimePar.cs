/* 
 *时间对
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

namespace CommonLibrary.Entities
{
    public class DateTimePair
    {
        public DateTimePair()
        { 

        }

        public DateTimePair(DateTime dt1, DateTime dt2)
        {
            _Start = dt1;
            _End = dt2;
        }

        private DateTime _Start;
        public DateTime Start
        {
            set { _Start = value; }
            get { return _Start; }
        }

        private DateTime _End;
        public DateTime End
        {
            set { _End = value; }
            get { return _End; }
        }
    }
}
