using Business.Requests;
using Business.Responses;

namespace Business.Services
{
    public interface IRegistry
    {
        void Store(StoreRegistryRequest request);

        GetServicesRegistryResponse GetServices(GetServicesRegistryRequest request);
    }
}