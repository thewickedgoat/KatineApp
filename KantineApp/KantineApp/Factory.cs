using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KantineApp.DLL;
using KantineApp.Entity;
using KantineApp.Interface;
using KantineApp.Gateway;

namespace KantineApp
{
    public class Factory
    {
        private static ServiceGateway _serviceGateway;
        private static MockRepository _repo;

        /// <summary>
        /// Static method enables calling the class directly instead of creating a new instance of it for access. 
        /// Get existing instance or create one if it does not exist.        
        /// </summary>
        public static IRepository GetRepository
        {
            get { return _repo ?? (_repo = new MockRepository()); }
        }

        public static IServiceGateway GetServiceGateway
        {
            get { return _serviceGateway ?? (_serviceGateway = new ServiceGateway());  }
        }
    }
}
