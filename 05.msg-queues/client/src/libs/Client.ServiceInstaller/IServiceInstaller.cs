namespace Client.ServiceInstaller
{
    public interface IServiceInstaller
    {
        void Install(ServiceOptions options);

        void Uninstall(ServiceOptions options);

        void Start(string serviceName);

        void Stop(string serviceName);

        bool IsServiceExists(string serviceName);
    }
}
