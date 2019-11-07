using System;
using System.Runtime.InteropServices;
using PowerManagement.PowerManagement;
using PowerManagement.PowerManagement.Enums;

namespace PowerManagement
{
    [ComVisible(true)]
    [Guid("06BA1AB8-D0CD-4023-B69D-3174C80C5073")]
    [ClassInterface(ClassInterfaceType.None)]
    public class SuspendManagementProvider : PowerManagementProviderBase, ISuspendManagementProvider
    {
        public void ReserveHibernationFile()
        {
            SetPowerInformationBooleanValue(
                PowerInformationLevel.SystemReserveHiberFile,
                value: true);
        }

        public void RemoveHibernationFile()
        {
            SetPowerInformationBooleanValue(
                PowerInformationLevel.SystemReserveHiberFile,
                value: false);
        }

        public void PutMachineHibernate()
        {
            PowrprofDllImport.SetSuspendState(
                bHibernate: true,
                bForce: false,
                bWakeupEventsDisabled: false);
        }

        public void PutMachineSleep()
        {
            PowrprofDllImport.SetSuspendState(
                bHibernate: false,
                bForce: false,
                bWakeupEventsDisabled: false);
        }

        private void SetPowerInformationBooleanValue(PowerInformationLevel level, bool value)
        {
            var inputBuffer = AllocateCoTaskBuffer<bool>(out var inputBufferSize);

            var byteValue = value ? (byte) 1 : (byte) 0;
            Marshal.WriteByte(inputBuffer, ofs: 0, val: byteValue);

            try
            {
                var returnCode = PowrprofDllImport
                    .CallNtPowerInformation(
                        level,
                        inputBuffer,
                        (ulong)inputBufferSize,
                        IntPtr.Zero,
                        0);

                EnsureWin32CallSucceeded(returnCode);
            }
            finally
            {
                Marshal.FreeCoTaskMem(inputBuffer);
            }
        }
    }
}
