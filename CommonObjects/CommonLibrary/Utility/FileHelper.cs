/* 
 * �ļ���λ��
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
        /// �������ʽ��ȡ�ļ�����
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadFile(string filePath, Encoding encoding)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                //�ļ�ָ��ָ��0λ��
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
