namespace Business.Requests
{
    public sealed class StoreRegistryRequest
    {
        public string ServiceName { get; set; }

        public string ServiceId { get; set; }

        public ulong TransactionId { get; set; }
    }
}