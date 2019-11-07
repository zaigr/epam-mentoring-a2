using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using PowerManagement.PowerManagement.Enums;

namespace PowerManagement.PowerManagement
{
    public abstract class PowerManagementProviderBase
    {
        protected internal IntPtr AllocateCoTaskBuffer<T>(out int bufferSize)
        {
            bufferSize = Marshal.SizeOf(typeof(T));

            return Marshal.AllocCoTaskMem(bufferSize);
        }

        protected internal void EnsureWin32CallSucceeded(uint returnCode)
        {
            if (returnCode != (uint)ReturnCode.Success)
            {
                var error = Marshal.GetLastWin32Error();
                throw new Win32Exception($"Win32 function invocation returned with error code: '{error}'");
            }
        }
    }
}
