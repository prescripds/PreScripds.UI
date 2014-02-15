using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Net;
using System.ServiceModel.Dispatcher;

namespace PreScripds.Infrastructure.Services
{
    class DynamicFormatter : IDispatchMessageFormatter
    {
        public IDispatchMessageFormatter jsonDispatchMessageFormatter { get; set; }
        public IDispatchMessageFormatter xmlDispatchMessageFormatter { get; set; }

        public void DeserializeRequest(System.ServiceModel.Channels.Message message, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public System.ServiceModel.Channels.Message SerializeReply(MessageVersion messageVersion, object[] parameters, object result)
        {
            Message request = OperationContext.Current.RequestContext.RequestMessage;

            // This code is based on ContentTypeBasedDispatch example in WCF REST Starter Kit Samples
            // It calls either 
            HttpRequestMessageProperty prop = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];

            string accepts = prop.Headers[HttpRequestHeader.Accept];
            if (accepts != null)
            {
                if (accepts.Contains("text/xml") || accepts.Contains("application/xml"))
                {
                    return xmlDispatchMessageFormatter.SerializeReply(messageVersion, parameters, result);
                }
                else if (accepts.Contains("application/json"))
                {
                    return jsonDispatchMessageFormatter.SerializeReply(messageVersion, parameters, result);
                }
            }
            else
            {
                string contentType = prop.Headers[HttpRequestHeader.ContentType];
                if (contentType != null)
                {
                    if (contentType.Contains("text/xml") || contentType.Contains("application/xml"))
                    {
                        return xmlDispatchMessageFormatter.SerializeReply(messageVersion, parameters, result);
                    }
                    else if (contentType.Contains("application/json"))
                    {
                        return jsonDispatchMessageFormatter.SerializeReply(messageVersion, parameters, result);
                    }
                }
            }
            return xmlDispatchMessageFormatter.SerializeReply(messageVersion, parameters, result);
        }

    }
}
