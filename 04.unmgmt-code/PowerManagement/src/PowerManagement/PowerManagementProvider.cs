using System;
using System.Runtime.InteropServices;
using PowerManagement.PowerManagement;
using PowerManagement.PowerManagement.Enums;
using PowerManagement.PowerManagement.Types;

namespace PowerManagement
{
    public class PowerManagementProvider : PowerManagementProviderBase, IPowerManagementProvider
    {
        public TimeSpan GetLastSleepTime()
        {
            var result = GetPowerInformationInt64Value(
                PowerInformationLevel.LastSleepTime);

            var seconds = GetMillisecondsFromNanosecondsTicks(result);

            return TimeSpan.FromMilliseconds(seconds);
        }

        public TimeSpan GetLastWakeTime()
        {
            var result = GetPowerInformationInt64Value(
                PowerInformationLevel.LastWakeTime);

            var milliseconds = GetMillisecondsFromNanosecondsTicks(result);

            return TimeSpan.FromMilliseconds(milliseconds);
        }

        public SystemBatteryState GetSystemBatteryState()
        {
            return GetPowerInformationStructure<SystemBatteryState>(
                PowerInformationLevel.SystemBatteryState);
        }

        public SystemPowerInformation GetSystemPowerInformation()
        {
            return GetPowerInformationStructure<SystemPowerInformation>(
                PowerInformationLevel.SystemPowerInformation);
        }

        private long GetPowerInformationInt64Value(PowerInformationLevel level)
        {
            var outputBuffer = AllocateCoTaskBuffer<long>(out var outputBufferSize);
            try
            {
                // Result returned in count of 100 nanoseconds
                var returnCode = PowrprofDllImport.CallNtPowerInformation(
                    level,
                    (IntPtr)null,
                    0,
                    outputBuffer,
                    (ulong)outputBufferSize);
                EnsureWin32CallSucceeded(returnCode);

                return Marshal.ReadInt64(outputBuffer, 0);
            }
            finally
            {
                Marshal.FreeCoTaskMem(outputBuffer);
            }
        }

        private T GetPowerInformationStructure<T>(PowerInformationLevel level)
            where T : struct
        {
            var outputBuffer = AllocateCoTaskBuffer<T>(out var outputBufferSize);
            try
            {
                var returnCode = PowrprofDllImport.CallNtPowerInformation(
                    level,
                    (IntPtr)null,
                    0,
                    outputBuffer,
                    (ulong)outputBufferSize);
                EnsureWin32CallSucceeded(returnCode);

                return Marshal.PtrToStructure<T>(outputBuffer);
            }
            finally
            {
                Marshal.FreeCoTaskMem(outputBuffer);
            }
        }

        private long GetMillisecondsFromNanosecondsTicks(long nanosecondsTicks)
        {
            return nanosecondsTicks / 100_000_000;
        }
    }
}
