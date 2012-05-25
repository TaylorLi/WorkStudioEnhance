/* 
 * 日志访问类
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
    public class LogHelper
    {
        private static log4net.ILog mainLog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static bool setted = false;
        public enum LogTypes
        {
            Error,
            Warning,
            Info
        }

        public static void SetConfig(string path)
        {
            if (!setted)
            {
                setted = true;
                if (string.IsNullOrEmpty(path))
                    log4net.Config.XmlConfigurator.Configure();
                else
                    log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(path));
            }
        }

        public static void Write(string logger, LogTypes logType, string message)
        {
            log4net.ILog log = mainLog;
            if (!string.IsNullOrEmpty(logger))
            {
                log = log4net.LogManager.GetLogger(logger);
            }
            switch (logType)
            {
                case LogTypes.Error:
                    log.Error(message);
                    break;
                case LogTypes.Warning:
                    log.Warn(message);
                    break;
                case LogTypes.Info:
                    log.Info(message);
                    break;
                default:
                    break;
            }

        }

        public static void Write(LogTypes logType, string message)
        {
            Write(null, logType, message);
        }

        public static void WriteWarn(string logger, string message)
        {
            Write(logger, LogTypes.Warning, message);
        }

        public static void WriteError(string logger, string message)
        {
            Write(logger, LogTypes.Error, message);
        }

        public static void WriteInfo(string logger, string message)
        {
            Write(logger, LogTypes.Info, message);
        }

        public static void WriteWarn(object logger, string message)
        {
            Write(logger.ToString(), LogTypes.Warning, message);
        }

        public static void WriteError(object logger, string message)
        {
            Write(logger.ToString(), LogTypes.Error, message);
        }

        public static void WriteInfo(object logger, string message)
        {
            Write(logger.ToString(), LogTypes.Info, message);
        }
    }
}
