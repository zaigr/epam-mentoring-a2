using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.ScanService.Monitoring.EventArgs;

namespace Client.ScanService.Monitoring
{
    public interface IResourceMonitor
    {
        event EventHandler<ResourceAddedEventArgs> ResourceAdded;

        event EventHandler<ResourceChangedEventArgs> ResourceChanged;

        Task StartMonitoring(IEnumerable<string> resourcePaths);
    }
}
