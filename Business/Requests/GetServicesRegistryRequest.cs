namespace Business.Requests
{
    public sealed class GetServicesRegistryRequest
    {
        public string ServiceName { get; set; }

        public ulong MinimalTransaction { get; set; }
    }
}