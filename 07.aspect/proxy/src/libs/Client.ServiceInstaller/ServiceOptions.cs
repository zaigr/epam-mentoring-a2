using System.ServiceProcess;

namespace Client.ServiceInstaller
{
    public class ServiceOptions
    {
        public string AssemblyPath { get; set; }

        public string ServiceName { get; set; }

        public string DisplayName { get; set; }

        public ServiceStartMode ServiceStartMode { get; set; }

        public string Description { get; set; }
    }
}
