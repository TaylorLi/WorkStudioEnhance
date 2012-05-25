/* 
 * 加密类
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
using System.Security.Cryptography;
using System.IO;

namespace CommonLibrary.Utility
{
    public class SecretHelper
    {
        public SecretHelper()
        {
        }
        /// <summary>
        /// Des加密，使用固定的IV（偏移值），输出字母形式的字符串
        /// </summary>
        /// <param name="strText">加密字符串</param>
        /// <param name="strEncrKey">密钥</param>
        /// <returns></returns>
        public static string DesEncrypt(string strText, string strEncrKey)
        {
            if (string.IsNullOrEmpty(strText) || string.IsNullOrEmpty(strEncrKey)) return strText;
            byte[] byKey = null;
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            byKey = System.Text.Encoding.UTF8.GetBytes(strEncrKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(strText);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }
        /// <summary>
        /// Des解密，使用固定的IV值（偏移值)
        /// </summary>
        /// <param name="strText">加密字符串</param>
        /// <param name="sDecrKey">密钥</param>
        /// <returns></returns>
        public static string DesDecrypt(string strText, string sDecrKey)
        {
            if (string.IsNullOrEmpty(strText) || string.IsNullOrEmpty(sDecrKey)) return strText;
            byte[] byKey = null;
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            byte[] inputByteArray = new Byte[strText.Length];
            byKey = System.Text.Encoding.UTF8.GetBytes(sDecrKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(strText);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetString(ms.ToArray());
        }
        /// <summary>
        /// Des加密，使用密钥作为IV（偏移值），输出16进制的字符串
        /// </summary>
        /// <param name="strText">加密字符串</param>
        /// <param name="strEncrKey">密钥</param>
        /// <returns></returns>
        public static string DesEncrypt2(string strToEncrypt, string encrKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(strToEncrypt);
            des.Key = ASCIIEncoding.UTF8.GetBytes(encrKey);
            des.IV = ASCIIEncoding.UTF8.GetBytes(encrKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            //Write  the  byte  array  into  the  crypto  stream    
            //(It  will  end  up  in  the  memory  stream)    
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            //Get  the  data  back  from  the  memory  stream,  and  into  a  string    
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                //Format  as  hex    
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }
        /// <summary>
        /// Des解密，使用密钥作为IV值（偏移值)，解密16进制形式的字符串
        /// </summary>
        /// <param name="strText">16进制形式的加密字符串</param>
        /// <param name="sDecrKey">密钥</param>
        /// <returns></returns>
        public static string DesDecrypt2(string strToDecrypt, string decrKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[strToDecrypt.Length / 2];
            for (int x = 0; x < strToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(strToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            des.Key = ASCIIEncoding.ASCII.GetBytes(MD5Encrypt(decrKey).Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(MD5Encrypt(decrKey).Substring(0, 8));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();

            return System.Text.Encoding.UTF8.GetString(ms.ToArray());
        }

        public void DesEncrypt(string m_InFilePath, string m_OutFilePath, string strEncrKey)
        {
            byte[] byKey = null;
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            byKey = System.Text.Encoding.UTF8.GetBytes(strEncrKey.Substring(0, 8));
            FileStream fin = new FileStream(m_InFilePath, FileMode.Open, FileAccess.Read);
            FileStream fout = new FileStream(m_OutFilePath, FileMode.OpenOrCreate, FileAccess.Write);
            fout.SetLength(0);
            byte[] bin = new byte[100];
            long rdlen = 0;
            long totlen = fin.Length;
            int len;
            DES des = new DESCryptoServiceProvider();
            CryptoStream encStream = new CryptoStream(fout, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
            while (rdlen < totlen)
            {
                len = fin.Read(bin, 0, 100);
                encStream.Write(bin, 0, len);
                rdlen = rdlen + len;
            }
            encStream.Close();
            fout.Close();
            fin.Close();
        }

        public void DesDecrypt(string m_InFilePath, string m_OutFilePath, string sDecrKey)
        {
            byte[] byKey = null;
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            byKey = System.Text.Encoding.UTF8.GetBytes(sDecrKey.Substring(0, 8));
            FileStream fin = new FileStream(m_InFilePath, FileMode.Open, FileAccess.Read);
            FileStream fout = new FileStream(m_OutFilePath, FileMode.OpenOrCreate, FileAccess.Write);
            fout.SetLength(0);

            byte[] bin = new byte[100];
            long rdlen = 0;
            long totlen = fin.Length;
            int len;

            DES des = new DESCryptoServiceProvider();
            CryptoStream encStream = new CryptoStream(fout, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);

            while (rdlen < totlen)
            {
                len = fin.Read(bin, 0, 100);
                encStream.Write(bin, 0, len);
                rdlen = rdlen + len;
            }

            encStream.Close();
            fout.Close();
            fin.Close();
        }

        public static string MD5Encrypt(string strText)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(strText));
            StringBuilder sb = new StringBuilder();
            // Loop through each byte of the hashed data    
            // and format each one as a hexadecimal string.   
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("x2"));
            }
            // Return the hexadecimal string.   
            return sb.ToString();
        }
        /// <summary>
        /// 根据指定的密码和哈希算法生成一个适合于存储在配置文件中的哈希密码。
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string MD5EncryptByFormSecurity(string strText)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strText, "MD5");
        }
    }
}
