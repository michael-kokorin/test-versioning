namespace Business.Responses
{
    public sealed class ServiceResponseModel
    {
        public string ServiceId { get; set; }

        public ulong LastTransaction { get; set; }
    }
}