using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Business.Models;
using Business.Responses;

namespace Business.Services.Impl
{
    public sealed class ServiceRegistry: IServiceRegistry
    {
        private readonly ConcurrentDictionary<string, VersionStorageModel> _registry = new ConcurrentDictionary<string, VersionStorageModel>();
        
        public void Store(string id, ServiceTransactionVersion serviceTransactionVersion)
        {
            _registry.AddOrUpdate(id, new VersionStorageModel
            {
                LastTransaction = serviceTransactionVersion.LastTransaction
            }, (key, model) =>
            {
                model.LastTransaction = serviceTransactionVersion.LastTransaction;
                return model;
            });
        }

        public IEnumerable<ServiceResponseModel> Get(ServiceTransactionVersion minimalRequirement)
        {
            return _registry.ToArray()
                .Where(_ => _.Value.LastTransaction.HasValue && _.Value.LastTransaction >= minimalRequirement.LastTransaction)
                .Select(_ => new ServiceResponseModel
                {
                    ServiceId = _.Key,
                    LastTransaction = _.Value.LastTransaction.Value
                });
        }

        private sealed class VersionStorageModel
        {
            public ulong? LastTransaction { get; set; }
        }
    }
}