/* 
 * 多语言管理
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

namespace CommonLibrary.WebObject
{
    public class CultureHelper
    {
        public static void SetCurrentLanguage(string language)
        {
            if (!string.IsNullOrEmpty(language))
            {
                language = language.Trim().ToLower();
                if ("zh-tw".Equals(language) || "zh-cn".Equals(language) || "en-us".Equals(language))
                {
                    System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(language);
                    if (ci != null)
                    {
                        System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
                    }
                }
            }
        }
    }
}
