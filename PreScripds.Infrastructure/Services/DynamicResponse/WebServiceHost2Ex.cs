using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Description;
using System.ServiceModel.Web;

namespace PreScripds.Infrastructure.Services
{
    class WebServiceHost2Ex : WebServiceHost
    {
        public WebServiceHost2Ex(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses) {}

        protected override void OnOpening()
        {
            base.OnOpening();

            foreach (var ep in Description.Endpoints)
            {
                if (ep.Behaviors.Find<WebHttpBehavior>() != null)
                {
                    ep.Behaviors.Remove<WebHttpBehavior>();
                    ep.Behaviors.Add(new WebHttpBehavior2Ex());
                }
            }
        }
    }
}
