namespace Client.ScanService.Configuration.External
{
    public interface IExternalConfigurationProvider
    {
        void SyncExternalConfiguration(ServiceConfig config);
    }
}
