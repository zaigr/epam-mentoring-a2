using System;
using System.Collections;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;

namespace Client.ServiceInstaller
{
    public class WindowsServiceInstaller : IServiceInstaller
    {
        public void Install(ServiceOptions options)
        {
            var service = GetServiceController(options.ServiceName);
            if (service != null)
            {
                throw new InvalidOperationException(
                    $"Can not install service '{options.ServiceName}'. It is already installed.");
            }

            var installer = GetInstaller(options);

            installer.Install(new Hashtable());
        }

        public void Uninstall(ServiceOptions options)
        {
            var service = GetServiceController(options.ServiceName);
            if (service == null)
            {
                throw new InvalidOperationException(
                    $"Can not uninstall service '{options.ServiceName}'. It does not exist.");
            }

            var installer = GetInstaller(options);
            installer.Uninstall(null);
        }

        public void Start(string serviceName)
        {
            var service = GetServiceController(serviceName);
            if (service == null)
            {
                throw new InvalidOperationException(
                    $"Cannot find service '{serviceName}' to start. Ensure service installed.");
            }

            service.Start();
        }

        public void Stop(string serviceName)
        {
            var service = GetServiceController(serviceName);
            if (service == null)
            {
                throw new InvalidOperationException(
                    $"Cannot find service '{serviceName}' to stop. Ensure service `installed.");
            }

            service.Stop();
        }

        public bool IsServiceExists(string serviceName)
        {
            return GetServiceController(serviceName) != null;
        }

        private ServiceController GetServiceController(string serviceName)
        {
            return ServiceController
                .GetServices()
                .FirstOrDefault(s => s.ServiceName == serviceName);
        }

        private TransactedInstaller GetInstaller(ServiceOptions options)
        {
            var serviceProcessInstaller = new ServiceProcessInstaller { Account = ServiceAccount.LocalSystem };
            var serviceInstaller = new System.ServiceProcess.ServiceInstaller
            {
                ServiceName = options.ServiceName,
                DisplayName = options.DisplayName,
                Description = options.Description,
                StartType = options.ServiceStartMode,
            };

            var transactedInstaller = new TransactedInstaller();
            transactedInstaller.Installers.Add(serviceProcessInstaller);
            transactedInstaller.Installers.Add(serviceInstaller);
            transactedInstaller.Context = new InstallContext(
                $"{options.ServiceName}-install.log",
                commandLine: new[] { $"/assemblypath={options.AssemblyPath}" });

            return transactedInstaller;
        }
    }
}
