/* 
 * Session访问类
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
    public class SessionHelper
    {
        public static void SetValue(string key, object value)
        {
            if (value != null)
                System.Web.HttpContext.Current.Session[key] = value;
        }
        public static object GetValue(string key)
        {
            return System.Web.HttpContext.Current.Session[key];
        }

        public static T GetSession<T>() where T : class, new()
        {
            string key = typeof(T).ToString();
            return GetValue(key) as T;
        }

        public static void SetValue<T>(T t)
        {
            string key = typeof(T).ToString();
            SetValue(key, t);
        }

        public static void Clear(string key)
        {
            if (!string.IsNullOrEmpty(key))
                System.Web.HttpContext.Current.Session.Remove(key);
        }
        public static void Clear<T>() where T : class, new()
        {
            string key = typeof(T).ToString();
            Clear(key);
        }
    }
}
