using System;
using System.Runtime.InteropServices;
using PowerManagement.PowerManagement.Types;

namespace PowerManagement
{
    [ComVisible(true)]
    [Guid("9F1645B3-D2D8-49A2-98CD-C0897BC2AE37")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IPowerManagementProvider
    {
        TimeSpan GetLastSleepTime();

        TimeSpan GetLastWakeTime();

        SystemBatteryState GetSystemBatteryState();

        SystemPowerInformation GetSystemPowerInformation();
    }
}
