using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;

namespace CommonLibrary.Extension.WcfExtentions
{
    /// <summary>
    /// 附加行为到SOAP请求或者响应
    /// </summary>
    public class MessageInspector : IClientMessageInspector, IDispatchMessageInspector
    {
        string logPath = "";
        string url = "";
        #region IClientMessageInspector 成员

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            MessageLogger.LogMessage(reply, ref logPath, ref url, "Client Receive Reply", false);
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            MessageLogger.LogMessage(request, ref logPath, ref url, "Client Send Request", true);
            return null;
        }

        #endregion

        #region IDispatchMessageInspector 成员

        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            MessageLogger.LogMessage(request, ref logPath, ref url, "Server Receive Request", true);
            return null;
        }

        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            MessageLogger.LogMessage(reply, ref logPath, ref url, "Server Send Reply", false);
        }

        #endregion
    }
}
