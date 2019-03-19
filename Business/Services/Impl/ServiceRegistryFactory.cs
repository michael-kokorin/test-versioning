namespace Business.Services.Impl
{
    public sealed class ServiceRegistryFactory: IServiceRegistryFactory
    {
        public IServiceRegistry Get()
        {
            return new ServiceRegistry();
        }
    }
}