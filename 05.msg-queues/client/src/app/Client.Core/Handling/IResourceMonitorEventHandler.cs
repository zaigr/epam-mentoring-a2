using Client.Core.Monitoring.EventArgs;

namespace Client.Core.Handling
{
    public interface IResourceMonitorEventHandler
    {
        void ResourceAddedEventHandler(object sender, ResourceAddedEventArgs eventArgs);

        void ResourceChangedEventHandler(object sender, ResourceChangedEventArgs eventArgs);
    }
}
