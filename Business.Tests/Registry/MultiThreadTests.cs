using System.Linq;
using System.Threading.Tasks;
using Business.Requests;
using Business.Services;
using Business.Services.Impl;
using NUnit.Framework;

namespace Business.Tests.Registry
{
    public class MultiThreadTests
    {
        private IRegistry _registry;
        
        [SetUp]
        public void Setup()
        {
            var serviceRegistryFactory = new ServiceRegistryFactory();
            _registry = new Services.Impl.Registry(serviceRegistryFactory);
        }

        [Test]
        public void MultiThreadTest()
        {
            const string serviceName = "Service_1";
            const string serviceId1 = "Id_1";
            const string serviceId2 = "Id_2";
            const int counter1 = 10;
            const int counter2 = 20;
            
            void Handler(string serviceId, int counter)
            {
                for (var index = 0; index <= counter; index++)
                {
                    _registry.Store(new StoreRegistryRequest
                    {
                        ServiceName = serviceName,
                        ServiceId = serviceId,
                        TransactionId = (ulong)index
                    });
                }
            }

            var task1 = Task.Run(() => Handler(serviceId1, counter1));
            var task2 = Task.Run(() => Handler(serviceId2, counter2));

            Task.WaitAll(task1, task2);

            var result = _registry.GetServices(new GetServicesRegistryRequest
            {
                ServiceName = serviceName,
                MinimalTransaction = 0
            });
            
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.Services);
            Assert.AreEqual(2, result.Services.Count());

            var service1 = result.Services.First();
            var service2 = result.Services.Skip(1).First();
            if (service1.ServiceId == serviceId1)
            {
                Assert.AreEqual(counter1, service1.LastTransaction);
                Assert.AreEqual(counter2, service2.LastTransaction);
            }
            else
            {
                Assert.AreEqual(counter2, service1.LastTransaction);
                Assert.AreEqual(counter1, service2.LastTransaction);
            }
        }
    }
}