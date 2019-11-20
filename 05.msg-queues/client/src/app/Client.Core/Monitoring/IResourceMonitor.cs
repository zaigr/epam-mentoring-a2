using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Core.Monitoring.EventArgs;

namespace Client.Core.Monitoring
{
    public interface IResourceMonitor
    {
        event EventHandler<ResourceAddedEventArgs> ResourceAdded;

        event EventHandler<ResourceChangedEventArgs> ResourceChanged;

        Task StartMonitoring(IEnumerable<string> resourcePaths);
    }
}
