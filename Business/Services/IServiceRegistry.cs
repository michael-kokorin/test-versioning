using System.Collections.Generic;
using Business.Models;
using Business.Responses;

namespace Business.Services
{
    public interface IServiceRegistry
    {
        void Store(string id, ServiceTransactionVersion serviceTransactionVersion);

        IEnumerable<ServiceResponseModel> Get(ServiceTransactionVersion minimalRequirement);
    }
}