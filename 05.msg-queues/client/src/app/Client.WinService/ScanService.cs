using System.ServiceProcess;

namespace Client.ScanService
{
    public partial class ScanService : ServiceBase
    {
        public ScanService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ServiceLocator.Start();
        }
    }
}
