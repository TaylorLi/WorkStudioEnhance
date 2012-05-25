/* 
 * 缓存MappingInfo，避免多次反射
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

namespace DataMapping
{
    internal static class MappingInfoCache
    {
        private static Dictionary<string, List<PropertyMappingInfo>> cache = new Dictionary<string, List<PropertyMappingInfo>>();
        internal static List<PropertyMappingInfo> GetCache(string typeName)
        {
            List<PropertyMappingInfo> info = null;
            try
            {
                //增加集合是否存在关键字的判断    modi by zhongsihui 20120221
                if (cache.ContainsKey(typeName))
                    info = (List<PropertyMappingInfo>)cache[typeName];
            }
            catch (KeyNotFoundException) { }

            return info;
        }

        internal static void SetCache(string typeName, List<PropertyMappingInfo> mappingInfoList)
        {
            try
            {
                cache[typeName] = mappingInfoList;
            }
            catch
            {
                cache = new Dictionary<string, List<PropertyMappingInfo>>();
            }
        }

        public static void ClearCache()
        {
            cache.Clear();
        }
    }
}
