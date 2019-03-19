using System.Linq;
using Business.Requests;
using Business.Services;
using Business.Services.Impl;
using NUnit.Framework;

namespace Business.Tests.Registry
{
    public class SingleValueTests
    {
        private IRegistry _registry;
        
        [SetUp]
        public void Setup()
        {
            var serviceRegistryFactory = new ServiceRegistryFactory();
            _registry = new Services.Impl.Registry(serviceRegistryFactory);
        }

        [Test]
        public void AddSingleValue_ShouldFindIt()
        {
            const string serviceName = "Service_1";
            const string serviceId = "Id_1";
            const ulong transactionId = 1;
            
            _registry.Store(new StoreRegistryRequest
            {
                ServiceId = serviceId,
                ServiceName = serviceName,
                TransactionId = transactionId
            });

            var result = _registry.GetServices(new GetServicesRegistryRequest
            {
                ServiceName = serviceName,
                MinimalTransaction = transactionId
            });
            
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.Services);
            Assert.AreEqual(1, result.Services.Count());
            Assert.AreEqual(transactionId, result.Services.First().LastTransaction);
            Assert.AreEqual(serviceId, result.Services.First().ServiceId);
        }
        
        [Test]
        public void AddSingleValue_ShouldNotFindIt()
        {
            const string serviceName = "Service_1";
            const string serviceId = "Id_1";
            const ulong transactionId = 1;

            _registry.Store(new StoreRegistryRequest
            {
                ServiceId = serviceId,
                ServiceName = serviceName,
                TransactionId = transactionId
            });

            var result = _registry.GetServices(new GetServicesRegistryRequest
            {
                ServiceName = serviceName,
                MinimalTransaction = transactionId + 1 // increase transaction id higher than the last value
            });
            
            Assert.IsNotNull(result);
            Assert.IsEmpty(result.Services);
        }
        
        [Test]
        public void AddSingleValueWithUpdate_ShouldFindIt()
        {
            const string serviceName = "Service_1";
            const string serviceId = "Id_1";
            const ulong transactionId = 1;

            _registry.Store(new StoreRegistryRequest
            {
                ServiceId = serviceId,
                ServiceName = serviceName,
                TransactionId = transactionId
            });
            
            _registry.Store(new StoreRegistryRequest
            {
                ServiceId = serviceId,
                ServiceName = serviceName,
                TransactionId = transactionId + 1
            });

            var result = _registry.GetServices(new GetServicesRegistryRequest
            {
                ServiceName = serviceName,
                MinimalTransaction = transactionId + 1
            });
            
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.Services);
            Assert.AreEqual(1, result.Services.Count());
            Assert.AreEqual(transactionId + 1, result.Services.First().LastTransaction);
            Assert.AreEqual(serviceId, result.Services.First().ServiceId);
        }
        
        [Test]
        public void AddSingleValueWithUpdate_ShouldNotFindIt()
        {
            const string serviceName = "Service_1";
            const string serviceId = "Id_1";
            const ulong transactionId = 1;

            _registry.Store(new StoreRegistryRequest
            {
                ServiceId = serviceId,
                ServiceName = serviceName,
                TransactionId = transactionId
            });
            
            _registry.Store(new StoreRegistryRequest
            {
                ServiceId = serviceId,
                ServiceName = serviceName,
                TransactionId = transactionId + 1
            });

            var result = _registry.GetServices(new GetServicesRegistryRequest
            {
                ServiceName = serviceName,
                MinimalTransaction = transactionId + 2 // increase transaction id higher than the last value
            });
            
            Assert.IsNotNull(result);
            Assert.IsEmpty(result.Services);
        }
    }
}