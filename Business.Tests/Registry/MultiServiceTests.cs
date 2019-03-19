using System.Linq;
using Business.Requests;
using Business.Services;
using Business.Services.Impl;
using NUnit.Framework;

namespace Business.Tests.Registry
{
    public class MultiServiceTests
    {
        private IRegistry _registry;
        
        [SetUp]
        public void Setup()
        {
            var serviceRegistryFactory = new ServiceRegistryFactory();
            _registry = new Services.Impl.Registry(serviceRegistryFactory);
        }

        [Test]
        public void MultiService_FindFirst()
        {
            const string serviceName1 = "Service_1";
            const string serviceName2 = "Service_2";
            const string serviceId1 = "Id_1";
            const string serviceId2 = "Id_2";
            const ulong transactionId1 = 1;
            const ulong transactionId2 = 2;
            
            _registry.Store(new StoreRegistryRequest
            {
                ServiceId = serviceId1,
                ServiceName = serviceName1,
                TransactionId = transactionId1
            });
            
            _registry.Store(new StoreRegistryRequest
            {
                ServiceId = serviceId2,
                ServiceName = serviceName2,
                TransactionId = transactionId2
            });

            // search is only by service 1
            var result = _registry.GetServices(new GetServicesRegistryRequest
            {
                ServiceName = serviceName1,
                MinimalTransaction = transactionId1
            });
            
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.Services);
            Assert.AreEqual(1, result.Services.Count());
            Assert.AreEqual(transactionId1, result.Services.First().LastTransaction);
            Assert.AreEqual(serviceId1, result.Services.First().ServiceId);
        }
        
        [Test]
        public void MultiService_ShouldNotFindFirst()
        {
            const string serviceName1 = "Service_1";
            const string serviceName2 = "Service_2";
            const string serviceId1 = "Id_1";
            const string serviceId2 = "Id_2";
            const ulong transactionId1 = 1;
            const ulong transactionId2 = 2;
            
            _registry.Store(new StoreRegistryRequest
            {
                ServiceId = serviceId1,
                ServiceName = serviceName1,
                TransactionId = transactionId1
            });
            
            _registry.Store(new StoreRegistryRequest
            {
                ServiceId = serviceId2,
                ServiceName = serviceName2,
                TransactionId = transactionId2
            });

            // search is only by service 1. transactionId is higher than any value for this service
            var result = _registry.GetServices(new GetServicesRegistryRequest
            {
                ServiceName = serviceName1,
                MinimalTransaction = transactionId1 + 1
            });
            
            Assert.IsNotNull(result);
            Assert.IsEmpty(result.Services);
        }
        
        [Test]
        public void MultiServiceWithUpdateFirst_ShouldNotFindFirst()
        {
            const string serviceName1 = "Service_1";
            const string serviceName2 = "Service_2";
            const string serviceId1 = "Id_1";
            const string serviceId2 = "Id_2";
            const ulong transactionId1 = 1;
            const ulong transactionId2 = 2;
            
            _registry.Store(new StoreRegistryRequest
            {
                ServiceId = serviceId1,
                ServiceName = serviceName1,
                TransactionId = transactionId1
            });
            
            _registry.Store(new StoreRegistryRequest
            {
                ServiceId = serviceId2,
                ServiceName = serviceName2,
                TransactionId = transactionId2
            });
            
            _registry.Store(new StoreRegistryRequest
            {
                ServiceId = serviceId1,
                ServiceName = serviceName1,
                TransactionId = transactionId1 + 3
            });

            // search is only by service 1. transactionId is higher than original value for service1 by lower than update value - so should find it
            var result = _registry.GetServices(new GetServicesRegistryRequest
            {
                ServiceName = serviceName1,
                MinimalTransaction = transactionId1 + 1
            });
            
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.Services);
            Assert.AreEqual(1, result.Services.Count());
            Assert.AreEqual(transactionId1 + 3, result.Services.First().LastTransaction);
            Assert.AreEqual(serviceId1, result.Services.First().ServiceId);
        }
    }
}