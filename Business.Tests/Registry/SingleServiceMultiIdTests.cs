using System.Linq;
using Business.Requests;
using Business.Services;
using Business.Services.Impl;
using NUnit.Framework;

namespace Business.Tests.Registry
{
    public class SingleServiceMultiIdTests
    {
        private IRegistry _registry;
        
        [SetUp]
        public void Setup()
        {
            var serviceRegistryFactory = new ServiceRegistryFactory();
            _registry = new Services.Impl.Registry(serviceRegistryFactory);
        }

        [Test]
        public void SingleServiceMultiId_FindAll()
        {
            const string serviceName = "Service_1";
            const string serviceId1 = "Id_1";
            const string serviceId2 = "Id_2";
            const ulong transactionId1 = 1;
            const ulong transactionId2 = 2;
            
            _registry.Store(new StoreRegistryRequest
            {
                ServiceId = serviceId1,
                ServiceName = serviceName,
                TransactionId = transactionId1
            });
            
            _registry.Store(new StoreRegistryRequest
            {
                ServiceId = serviceId2,
                ServiceName = serviceName,
                TransactionId = transactionId2
            });

            // transactionId1 is less than transactionId2 - so should find all of service items
            var result = _registry.GetServices(new GetServicesRegistryRequest
            {
                ServiceName = serviceName,
                MinimalTransaction = transactionId1
            });
            
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.Services);
            Assert.AreEqual(2, result.Services.Count());
        }
        
        [Test]
        public void SingleServiceMultiId_FindOneOfThem()
        {
            const string serviceName = "Service_1";
            const string serviceId1 = "Id_1";
            const string serviceId2 = "Id_2";
            const ulong transactionId1 = 1;
            const ulong transactionId2 = 2;
            
            _registry.Store(new StoreRegistryRequest
            {
                ServiceId = serviceId1,
                ServiceName = serviceName,
                TransactionId = transactionId1
            });
            
            _registry.Store(new StoreRegistryRequest
            {
                ServiceId = serviceId2,
                ServiceName = serviceName,
                TransactionId = transactionId2
            });

            // transactionId1 is less than transactionId2 - so should find only the second item
            var result = _registry.GetServices(new GetServicesRegistryRequest
            {
                ServiceName = serviceName,
                MinimalTransaction = transactionId2
            });
            
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.Services);
            Assert.AreEqual(1, result.Services.Count());
            Assert.AreEqual(transactionId2, result.Services.First().LastTransaction);
            Assert.AreEqual(serviceId2, result.Services.First().ServiceId);
        }
        
        [Test]
        public void SingleServiceMultiId_ShouldNotFind()
        {
            const string serviceName = "Service_1";
            const string serviceId1 = "Id_1";
            const string serviceId2 = "Id_2";
            const ulong transactionId1 = 1;
            const ulong transactionId2 = 2;
            
            _registry.Store(new StoreRegistryRequest
            {
                ServiceId = serviceId1,
                ServiceName = serviceName,
                TransactionId = transactionId1
            });
            
            _registry.Store(new StoreRegistryRequest
            {
                ServiceId = serviceId2,
                ServiceName = serviceName,
                TransactionId = transactionId2
            });

            // transactionId2 + 1 is higher than any values - so should not find any of items
            var result = _registry.GetServices(new GetServicesRegistryRequest
            {
                ServiceName = serviceName,
                MinimalTransaction = transactionId2 + 1
            });
            
            Assert.IsNotNull(result);
            Assert.IsEmpty(result.Services);
        }
    }
}