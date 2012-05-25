/* 
 * Json������
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
using System.Data;
using System.Collections;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace CommonLibrary.WebObject
{
    public class JsonHelper
    {
        public static string ToJson(object o)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(o);
        }

        public static T FromJson<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public static string GetJsonResult(bool success, string message, object extraData)
        {
            return JsonHelper.ToJson(new JsonHelper.GeneralJsonResult() { success = success, message = message, data = extraData });
        }

        public class GeneralJsonResult
        {
            public GeneralJsonResult()
            {

            }

            public bool success { get; set; }
            public string message { get; set; }
            public object data { get; set; }
        }
    }
}
