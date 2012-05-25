using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.Collections.ObjectModel;
using System.ServiceModel.Channels;

namespace CommonLibrary.Extension.WcfExtentions
{
    /// <summary>
    /// 捕获Soap信息，并进行对应操作
    /// </summary>
    public class MessageLogBehavior : IEndpointBehavior, IServiceBehavior
    {
        #region IEndpointBehavior 成员

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new MessageInspector());
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new MessageInspector());
        }

        public void Validate(ServiceEndpoint endpoint)
        {

        }

        #endregion

        #region IServiceBehavior 成员
        // 摘要:
        //     用于向绑定元素传递自定义数据，以支持协定实现。
        //
        // 参数:
        //   serviceDescription:
        //     服务的服务说明。
        //
        //   serviceHostBase:
        //     服务的宿主。
        //
        //   endpoints:
        //     服务终结点。
        //
        //   bindingParameters:
        //     绑定元素可访问的自定义对象。
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {

        }

        //
        // 摘要:
        //     用于更改运行时属性值或插入自定义扩展对象（例如错误处理程序、消息或参数拦截器、安全扩展以及其他自定义扩展对象）。
        //
        // 参数:
        //   serviceDescription:
        //     服务说明。
        //
        //   serviceHostBase:
        //     当前正在生成的宿主。
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher chanDisp in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher dispacher in chanDisp.Endpoints)
                {
                    dispacher.DispatchRuntime.MessageInspectors.Add(new MessageInspector());
                }
            }
        }

        //
        // 摘要:
        //     用于检查服务宿主和服务说明，从而确定服务是否可成功运行。
        //
        // 参数:
        //   serviceDescription:
        //     服务说明。
        //
        //   serviceHostBase:
        //     当前正在构建的服务宿主。
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        #endregion
    }
}
