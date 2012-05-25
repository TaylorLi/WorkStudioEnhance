using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Configuration;
using System.ServiceModel;
using System.IO;
using System.Xml;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using CommonLibrary.Utility.Xml;

namespace CommonLibrary.Extension.WcfExtentions
{
    /// <summary>
    /// 记录SOAP消息
    /// </summary>
    internal class MessageLogger
    {
        static string LogsFolder = CommonLibrary.Utility.ConfigHelper.GetAppSetting("WcfLogs", "D:\\Logs\\Wcf");
        static bool EanbleWcfLog = CommonLibrary.Utility.ConfigHelper.GetBoolSetting("EanbleWcfLog");
        static bool WcfLogBodyOnly = CommonLibrary.Utility.ConfigHelper.GetBoolSetting("WcfLogBodyOnly");
        const string LogSplit = "-------------------------------------------------------------------------------------------";
        internal static void LogMessage(System.ServiceModel.Channels.Message msg, ref string postLogFilePath, ref string postUrl, string description, bool isFirstStep)
        {
            if (!EanbleWcfLog)
            {
                return;
            }
            try
            {
                if (msg == null)
                    return;
                string logPath = Path.Combine(LogsFolder, DateTime.Now.ToString("yyyy-MM-dd"));
                string logFile = string.Empty;
                string url = string.Empty;
                string action = "";

                #region Bind Log File Path
                if (!string.IsNullOrEmpty(postLogFilePath) && File.Exists(postLogFilePath) && !isFirstStep)
                {
                    logFile = postLogFilePath;
                    url = postUrl;
                    action = msg.Headers.Action;
                }
                else
                {
                    if (msg.Headers.From != null && msg.Headers.From.Uri != null && !string.IsNullOrEmpty(msg.Headers.From.Uri.AbsoluteUri))
                    {
                        url = msg.Headers.From.Uri.AbsoluteUri;
                        logPath = Path.Combine(logPath, GetValueForSplit(msg.Headers.From.Uri.AbsoluteUri, '/').Split('.')[0]);
                    }
                    if (url == string.Empty && msg.Headers.To != null && !string.IsNullOrEmpty(msg.Headers.To.AbsoluteUri))
                    {
                        url = msg.Headers.To.AbsoluteUri;
                        logPath = Path.Combine(logPath, GetValueForSplit(msg.Headers.To.AbsoluteUri, '/').Split('.')[0]);
                    }
                    if (!System.IO.Directory.Exists(logPath)) System.IO.Directory.CreateDirectory(logPath);
                    try
                    {
                        action = msg.Headers.Action;
                        if (!string.IsNullOrEmpty(action))
                        {
                            logFile = Path.Combine(logPath, GetValueForSplit(action, '/'));
                        }
                        else
                        {
                            action = "No-Action";
                            logFile = Path.Combine(logPath, action);
                        }
                    }
                    catch //(Exception ex)
                    {
                        action = "No-Action";
                        logFile = Path.Combine(logPath, action);
                    }
                    logFile += ".txt";
                }
                #endregion

                #region Write to Log File
                using (FileStream fs = new FileStream(logFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    StreamWriter w = new StreamWriter(fs);
                    w.WriteLine(LogSplit);
                    w.WriteLine(string.Concat("Message Type:", description));
                    w.WriteLine(string.Concat("Message in: ", msg.State.ToString()));
                    w.WriteLine(string.Concat("Action: ", msg.Headers.Action));
                    w.WriteLine(string.Concat("URL: ", url));
                    w.WriteLine(DateTime.Now);
                    w.WriteLine();
                    w.WriteLine("Content Start:");
                    w.Flush();

                    string xml = msg.ToString();
                    if (WcfLogBodyOnly)
                    {
                        string bodyXml = string.Empty;
                        foreach (XmlNode node in XmlHelper.GetDoc(xml).GetElementsByTagName("*"))
                        {
                            if (node.Name.EndsWith("Body"))
                            {
                                bodyXml = node.InnerXml != null ? node.InnerXml.Replace("&lt;", "<").Replace("&gt;", ">") : string.Empty;
                                break;
                            }
                        }
                        w.WriteLine(bodyXml);
                    }
                    else
                    {
                        w.WriteLine(xml);
                    }
                    w.Flush();

                    w.WriteLine("Content End");
                    w.WriteLine();
                    w.Flush();
                }
                #endregion

                if (isFirstStep)
                {
                    postLogFilePath = logFile;
                    postUrl = url;
                }
                else
                {
                    postLogFilePath = string.Empty;
                    postUrl = url;
                }
            }
            catch
            {
            }
        }
        static string GetValueForSplit(string v, char separator)
        {
            if (string.IsNullOrEmpty(v)) return v;
            string[] vi = v.Split('/');
            return vi[vi.Length - 1];
        }
    }
}
