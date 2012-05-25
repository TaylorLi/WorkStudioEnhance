/* 
 * 文件方位类
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
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CommonLibrary.Utility
{
    public class FileHelper
    {
        public static bool SerializeToFile(object serializeObject, string filePath, Encoding encoding)
        {
            if (File.Exists(filePath) == true)
            {
                File.Delete(filePath);
            }
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                XmlTextWriter writer = new XmlTextWriter(fileStream, encoding);
                writer.Formatting = Formatting.Indented;
                XmlSerializer xmlSerializer = new XmlSerializer(serializeObject.GetType());
                xmlSerializer.Serialize(writer, serializeObject);
                fileStream.Flush();
                fileStream.Close();
                return true;
            }
        }

        public static bool SerializeToFile(object serializeObject, string filePath)
        {
            return SerializeToFile(serializeObject, filePath, Encoding.Default);
        }

        public static object DeSerializeFromFile(string filePath, Type targetType)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fileStream.Position = 0;
                XmlSerializer xmlSerializer = new XmlSerializer(targetType);
                object result = xmlSerializer.Deserialize(fileStream);
                fileStream.Close();
                return result;
            }
        }

        public static void GenFile(string filePath, string sContent)
        {
            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                sw.WriteLine(sContent.ToString());
                sw.WriteLine();
                sw.Close();
            }
        }
        /// <summary>
        /// 共享的形式读取文件内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadFile(string filePath, Encoding encoding)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                //文件指针指向0位置
                StreamReader reader = new StreamReader(file, encoding);
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
