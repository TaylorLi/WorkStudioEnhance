/* 
 * 多语言
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

namespace CommonLibrary.Utility
{

    public class MutiLanguage
    {
        public enum Languages
        {
            en_us,
            zh_cn,
            zh_tw
        }

        public static readonly string[] LanguageStrings= { "en-us", "zh-cn", "zh-tw" };

        public static string EnumToString(Languages lang)
        {
            string language = "en-us";
            switch (lang)
            {
                case Languages.en_us:
                    language = "en-us";
                    break;
                case Languages.zh_cn:
                    language = "zh-cn";
                    break;
                case Languages.zh_tw:
                    language = "zh-tw";
                    break;
                default:
                    break;
            }
            return language;
        }

        public static Languages GetCultureType()
        {
            System.Globalization.CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentUICulture;
            string lang = null;
            if (ci != null) lang = ci.ToString().ToLower();

            if (lang == null || lang == "en-us")
                return Languages.en_us;
            else if (lang == "zh-cn")
                return Languages.zh_cn;
            else if (lang == "zh-tw")
                return Languages.zh_tw;
            else
                return Languages.en_us;
        }

        public static string GetLanguageString()
        {
            return EnumToString(GetCultureType());
        }      
    }
}
