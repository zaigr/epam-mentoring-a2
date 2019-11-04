using Xunit;
using Xunit.Abstractions;

namespace PowerManagement.Tests.Unit
{
    public class PowerManagementProviderTests
    {
        private readonly ITestOutputHelper _outputHelper;

        private readonly IPowerManagementProvider _provider = new PowerManagementProvider();

        public PowerManagementProviderTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public void GetLastSleepTimeTest()
        {
            var result = _provider.GetLastSleepTime();

            _outputHelper.WriteLine(result.ToString());
        }

        [Fact]
        public void GetLastWakeTimeTest()
        {
            var result = _provider.GetLastWakeTime();

            _outputHelper.WriteLine(result.ToString());
        }

        [Fact]
        public void GetSystemBatteryStateTest()
        {
            var result = _provider.GetSystemBatteryState();

            WritePropertyValues(result);
        }

        [Fact]
        public void GetSystemPowerInformationTest()
        {
            var result = _provider.GetSystemPowerInformation();

            WritePropertyValues(result);
        }

        private void WritePropertyValues<T>(T value)
        {
            foreach (var fieldInfo in typeof(T).GetFields())
            {
                _outputHelper.WriteLine($"{fieldInfo.Name}: {fieldInfo.GetValue(value)}");
            }
        }
    }
}
