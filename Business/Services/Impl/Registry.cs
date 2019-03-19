using System.Collections.Concurrent;
using Business.Models;
using Business.Requests;
using Business.Responses;

namespace Business.Services.Impl
{
    public sealed class Registry: IRegistry
    {
        private readonly IServiceRegistryFactory _serviceRegistryFactory;
        private readonly ConcurrentDictionary<string, IServiceRegistry> _registry = new ConcurrentDictionary<string, IServiceRegistry>();

        public Registry(IServiceRegistryFactory serviceRegistryFactory)
        {
            _serviceRegistryFactory = serviceRegistryFactory;
        }

        private IServiceRegistry Get(string serviceName)
        {
            return _registry.GetOrAdd(serviceName, _ => _serviceRegistryFactory.Get());
        }
        
        public void Store(StoreRegistryRequest request)
        {
            var service = Get(request.ServiceName);
            service.Store(request.ServiceId, new ServiceTransactionVersion
            {
                LastTransaction = request.TransactionId
            });
        }

        public GetServicesRegistryResponse GetServices(GetServicesRegistryRequest request)
        {
            var service = Get(request.ServiceName);
            var data = service.Get(new ServiceTransactionVersion
            {
                LastTransaction = request.MinimalTransaction
            });

            return new GetServicesRegistryResponse
            {
                Services = data
            };
        }
    }
}