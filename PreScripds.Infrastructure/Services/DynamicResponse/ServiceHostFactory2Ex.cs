using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Activation;
using System.ServiceModel;


namespace PreScripds.Infrastructure.Services
{
    public class ServiceHostFactory2Ex : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, 
                                                         Uri[] baseAddresses)
        {
            WebServiceHost2Ex webServiceHost2Ex = 
                new WebServiceHost2Ex(serviceType, baseAddresses);
            return webServiceHost2Ex;
        }
    }
}
