namespace Client.Core.Monitoring.EventArgs
{
    public class ResourceEventArgsBase
    {
        public string ResourceName { get; set; }

        public string ResourcePath { get; set; }

        public long ResourceBytesSize { get; set; }
    }
}
