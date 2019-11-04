using System;
using PowerManagement.PowerManagement.Types;

namespace PowerManagement
{
    public interface IPowerManagementProvider
    {
        TimeSpan GetLastSleepTime();

        TimeSpan GetLastWakeTime();

        SystemBatteryState GetSystemBatteryState();

        SystemPowerInformation GetSystemPowerInformation();
    }
}
