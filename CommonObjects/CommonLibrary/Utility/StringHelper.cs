/* 
 * �ַ����鳣�÷�������
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
using System.Text.RegularExpressions;
using System.Xml.XPath;
using System.IO;

namespace CommonLibrary.Utility
{
    public class StringHelper
    {
        public static string[] ObjectsToStrings(object[] objs)
        {
            List<string> listStr = new List<string>();
            foreach (object o in objs) listStr.Add(o.ToString());
            return listStr.ToArray();
        }

        public static Dictionary<string, string> String2Dictionary(string content, char split_main, char split_sub)
        {
            Dictionary<string, string> dct_ret = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(content))
            {
                foreach (string s in content.Split(split_main))
                {
                    string[] c = s.Split(split_sub);
                    if (c.Length == 2)
                    {
                        dct_ret[c[0].Trim()] = c[1];
                    }
                }
            }
            return dct_ret;
        }

        public static string ArrayToString(string[] array, string separate)
        {
            if (array == null) return string.Empty;
            StringBuilder sb_ret = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                if (sb_ret.Length == 0)
                    sb_ret.Append(array[i]);
                else
                    sb_ret.Append(separate).Append(array[i]);
            }
            return sb_ret.ToString();
        }

        public static string ObjectArrayToString(object[] array, string separate)
        {
            return ArrayToString(ObjectsToStrings(array), separate);
        }

        public static int[] StringArrayToIntArray(string[] array)
        {
            List<int> ret = new List<int>();
            foreach (string s in array)
                ret.Add(int.Parse(s));
            return ret.ToArray();
        }

        public static int[] String2IntArray(string paraStr, string preFix, char separator)
        {
            return StringArrayToIntArray(paraStr.Replace(preFix, "").Split(separator));
        }

        public static List<T> StringToList<T>(string str, char separator)
        {
            List<T> result = new List<T>();
            if (!string.IsNullOrEmpty(str) && str.Length > 0)
            {
                Type t = typeof(T);
                foreach (string s in str.Trim().Split(separator))
                {
                    if (!string.IsNullOrEmpty(s) && s.Trim().Length > 0)
                    {
                        T item;
                        try
                        {
                            item = (T)Convert.ChangeType(s.Trim(), t);
                        }
                        catch
                        {

                            continue;
                        }
                        if (!result.Contains(item))
                            result.Add(item);
                    }
                }
            }
            return result;
        }

        public static string[] GetParameterStringArray(string paraStr, string preFix, char separator)
        {
            return paraStr.Replace(preFix, "").Split(separator);
        }

        public static int[] String2IntArray(string str, char separator)
        {
            return StringArrayToIntArray(str.Split(separator));
        }

        public static string ArrayToString<T>(IEnumerable<T> array, string separate, string prepend, string append)
        {
            if (array == null) return string.Empty;
            StringBuilder sb_ret = new StringBuilder();
            foreach (T a in array)
            {
                if (sb_ret.Length != 0)
                {
                    sb_ret.Append(separate);
                }
                if (!string.IsNullOrEmpty(prepend))
                    sb_ret.Append(prepend);
                sb_ret.Append(a);
                if (!string.IsNullOrEmpty(append))
                    sb_ret.Append(append);
            }
            return sb_ret.ToString();
        }
        public static string ArrayToString<T>(IEnumerable<T> array, string separate)
        {
            return ArrayToString<T>(array, separate, null, null);
        }

        public static string GetRandomString(int size, bool lowerCase)
        {
            Random r = new Random(unchecked((int)DateTime.UtcNow.Ticks));
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException("size", "Size must be positive");
            }
            StringBuilder builder = new StringBuilder(size);
            int low = 65; // 'A'
            int high = 91; // 'Z' + 1
            if (lowerCase)
            {
                low = 97; // 'a';
                high = 123; // 'z' + 1
            }
            for (int i = 0; i < size; i++)
            {
                char ch = Convert.ToChar(r.Next(low, high));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        public static object Evaluate(string sExpression)
        {
            string xsltExpression = string.Format("number({0})",
                    new Regex(@"([\+\-\*])").Replace(sExpression, " ${1} ")
                                            .Replace("/", " div ")
                                            .Replace("%", " mod "));
            return new XPathDocument(new StringReader("<r/>")).CreateNavigator().Evaluate(xsltExpression);
        }

        /// <summary>
        /// ȥ��ԭ�ַ�����β���������滻�ַ���
        /// �磺ԭ�ַ���"sdlfjdcdcd",�滻�ַ���"cd" ����"sdlfjd"
        /// </summary>
        /// <param name="strSrc">Ҫ������ַ���</param>
        /// <param name="strTrim">ɾ�����ַ���</param>
        /// <returns>�������ַ���</returns>
        public static string TrimEnd(string strSrc, string strTrim)
        {
            if (strSrc.EndsWith(strTrim))
            {
                string strDes = strSrc.Substring(0, strSrc.Length - strTrim.Length);
                return TrimEnd(strDes, strTrim);
            }
            return strSrc;
        }
    }
}
