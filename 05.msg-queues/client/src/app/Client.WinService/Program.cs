using System;
using System.Reflection;
using System.ServiceProcess;
using Client.ServiceInstaller;

namespace Client.ScanService
{
    internal static class Program
    {
        public static void Main()
        {
#if DEBUG
            ServiceLocator.Start();

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();

            return;
#endif

            var serviceOptions = GetServiceOptions();

            var installer = new WindowsServiceInstaller();
            if (installer.IsServiceExists(serviceOptions.ServiceName))
            {
                Console.WriteLine($"Service '{serviceOptions.ServiceName}' already installed. Stop and remove it.");

                installer.Stop(serviceOptions.ServiceName);
                installer.Uninstall(serviceOptions);
            }

            Console.WriteLine($"Install and start '{serviceOptions.ServiceName}' service");

            installer.Install(serviceOptions);
            installer.Start(serviceOptions.ServiceName);

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }

        private static ServiceOptions GetServiceOptions()
        {
            var serviceAssemblyPath = Assembly.GetEntryAssembly()?.Location;
            if (serviceAssemblyPath == null)
            {
                throw new InvalidOperationException("Cannot find location of entry assembly.");
            }

            return new ServiceOptions
            {
                AssemblyPath = serviceAssemblyPath,
                ServiceName = nameof(ScanService),
                DisplayName = nameof(ScanService),
                Description = "Service scanning machine folders.",
                ServiceStartMode = ServiceStartMode.Automatic,
            };
        }
    }
}
