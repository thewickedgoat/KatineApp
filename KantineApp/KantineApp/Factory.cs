using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KantineApp.Entity;
using KantineApp.Interface;
using KantineApp.Gateway;

namespace KantineApp
{
    public class Factory
    {
        private static ServiceGateway _serviceGateway;

        public static IServiceGateway GetServiceGateway
        {
            get { return _serviceGateway ?? (_serviceGateway = new ServiceGateway());  }
        }
    }
}
