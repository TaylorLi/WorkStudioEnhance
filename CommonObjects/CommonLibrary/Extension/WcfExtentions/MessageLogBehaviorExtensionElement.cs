/*
 * Wcf协议信息记录类
 * 
 * 备注：1、Webconfig/AppConfig appSettings块中配置Key为WcfLogs的节点，可以控制信息保存路径,默认保存到D:\\Logs\\Wcf
 *       2、Webconifg system.serviceModel块中配置以下信息，调用本类
 *          添加节extensions/behaviorExtensions,在serviceBehaviors或者endpointBehaviors添加扩展MessageLogBehavior
 * <system.serviceModel>
 *  <behaviors>
      <serviceBehaviors>
        <behavior name="DataTransmission.DataPushWCFServiceBehavior">
          <serviceMetadata httpGetEnabled="true" />
          <serviceDebug includeExceptionDetailInFaults="true" />
		  <MessageLogBehavior />
        </behavior>
      </serviceBehaviors>
	  <endpointBehaviors>
		  <behavior name="EndpointLogBehavior">
    		<callbackDebug includeExceptionDetailInFaults="true"/>
			<MessageLogBehavior />
		  </behavior>
	  </endpointBehaviors>
    </behaviors>
 *    <extensions>
		  <behaviorExtensions>
			  <add name="MessageLogBehavior" type="CommonLibrary.Extension.WcfExtentions.MessageLogBehaviorExtensionElement, CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" />
		  </behaviorExtensions>
	  </extensions>
 * </system.serviceModel>
		1、对于client/extensions/endpoint	 behaviorConfiguration="EndpointLogBehavior"   
 * 如下:
 * <endpoint behaviorConfiguration="EndpointLogBehavior" address="http://219.141.242.4/dtws/DataTransmission.asmx" binding="basicHttpBinding" bindingConfiguration="DataTransmissionSoap" contract="PolicyService.DataTransmissionSoap" name="DataTransmissionSoap" />
 * 2、对于services节保持不变即可。
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Configuration;

namespace CommonLibrary.Extension.WcfExtentions
{
    /// <summary>
    /// WCF 扩展行为
    /// </summary>
    public class MessageLogBehaviorExtensionElement : BehaviorExtensionElement
    {

        public override Type BehaviorType
        {
            get { return typeof(MessageLogBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new MessageLogBehavior();
        }
    }
}
