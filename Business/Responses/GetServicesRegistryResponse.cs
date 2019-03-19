using System.Collections.Generic;

namespace Business.Responses
{
    public sealed class GetServicesRegistryResponse
    {
        public IEnumerable<ServiceResponseModel> Services { get; set; }
    }
}