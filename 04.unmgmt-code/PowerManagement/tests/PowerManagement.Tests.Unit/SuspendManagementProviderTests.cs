using Xunit;
using Xunit.Abstractions;

namespace PowerManagement.Tests.Unit
{
    public class SuspendManagementProviderTests
    {
        private readonly ITestOutputHelper _outputHelper;

        private readonly ISuspendManagementProvider _provider = new SuspendManagementProvider();

        public SuspendManagementProviderTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public void ReserveHibernationFileTest()
        {
            _provider.ReserveHibernationFile();
        }

        [Fact]
        public void RemoveHibernationFileTest()
        {
            _provider.RemoveHibernationFile();
        }

        [Fact]
        public void PutMachineHibernateTest()
        {
            _provider.PutMachineHibernate();
        }

        [Fact]
        public void PutMachineSleepTest()
        {
            _provider.PutMachineSleep();
        }
    }
}
