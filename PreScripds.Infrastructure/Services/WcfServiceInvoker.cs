using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Configuration;


namespace PreScripds.Infrastructure.Services
{
    /// <summary>
    ///     Sample Call
    ///     <code>
    /// WCFServiceInvoker invoker = new WCFServiceInvoker();
    /// SomeReturnType result = invoker.InvokeService<IMyServiceContract, SomeReturnType>
    ///                                                  (
    ///                                                  proxy => proxy.SomeMethod()
    ///                                                  );
    /// </code>
    /// </summary>
    public interface IServiceInvoker
    {

        TR InvokeService<T, TR>(Func<T, TR> invokeHandler)
            where T : class;
    }

    public class WcfServiceInvoker : IServiceInvoker
    {

        private static readonly ChannelFactoryManager _factoryManager = new ChannelFactoryManager();

        private static readonly ClientSection _clientSection =
            ConfigurationManager.GetSection("system.serviceModel/client") as ClientSection;

        #region IServiceInvoker Members

        public TResult InvokeService<T, TResult>(Func<T, TResult> invokeHandler)
            where T : class
        {
            var endpointNameAddressPair = GetEndpointNameAddressPair(typeof(T));
            var arg = _factoryManager.CreateChannel<T>(endpointNameAddressPair.Key, endpointNameAddressPair.Value);
            var obj2 = (ICommunicationObject)arg;
            try
            {
                return invokeHandler(arg);
            }
            finally
            {
                try
                {
                    if (obj2.State != CommunicationState.Faulted)
                    {
                        obj2.Close();
                    }
                }
                catch
                {
                    obj2.Abort();
                }
            }
        }

        #endregion

        public void InvokeService<T>(Action<T> invokeHandler) where T : class
        {
            var endpointNameAddressPair = GetEndpointNameAddressPair(typeof(T));
            var arg = _factoryManager.CreateChannel<T>(endpointNameAddressPair.Key, endpointNameAddressPair.Value);
            var obj2 = (ICommunicationObject)arg;
            try
            {
                invokeHandler(arg);
            }
            finally
            {
                try
                {
                    if (obj2.State != CommunicationState.Faulted)
                    {
                        obj2.Close();
                    }
                }
                catch
                {
                    obj2.Abort();
                }
            }
        }

        private KeyValuePair<string, string> GetEndpointNameAddressPair(Type serviceContractType)
        {
            var configException =
                new ConfigurationErrorsException(
                    string.Format(
                        "No client endpoint found for type {0}. Please add the section <client><endpoint name=\"myservice\" address=\"http://address/\" binding=\"basicHttpBinding\" contract=\"{0}\"/></client> in the config file.",
                        serviceContractType));
            if (((_clientSection == null) || (_clientSection.Endpoints == null)) || (_clientSection.Endpoints.Count < 1))
            {
                throw configException;
            }
            foreach (ChannelEndpointElement element in _clientSection.Endpoints)
            {
                if (element.Contract == serviceContractType.ToString())
                {
                    return new KeyValuePair<string, string>(element.Name, element.Address.AbsoluteUri);
                }
            }
            throw configException;
        }

    }
}
