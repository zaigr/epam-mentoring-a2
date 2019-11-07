using System;
using System.Runtime.InteropServices;
using PowerManagement.PowerManagement.Enums;

namespace PowerManagement.PowerManagement
{
    internal static class PowrprofDllImport
    {
        /// <summary>
        /// Sets or retrieves power information.
        /// </summary>
        /// <param name="informationLevel">
        /// The information level requested.
        /// This value indicates the specific power information to be set or retrieved.
        /// </param>
        /// <param name="lpInputBuffer">
        /// A pointer to an optional input buffer.
        /// The data type of this buffer depends on the information level requested in the InformationLevel parameter.
        /// </param>
        /// <param name="nInputBufferSize">
        /// The size of the input buffer, in bytes.
        /// </param>
        /// <param name="lpOutputBuffer">
        /// A pointer to an optional output buffer.
        /// The data type of this buffer depends on the information level requested in the InformationLevel parameter.
        /// </param>
        /// <param name="nOutputBufferSize">
        /// The size of the output buffer, in bytes.
        /// Depending on the information level requested, this may be a variably sized buffer.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is STATUS_SUCCESS.
        /// </returns>
        [DllImport("powrprof.dll", SetLastError = true)]
        public static extern uint CallNtPowerInformation(
            [In] PowerInformationLevel informationLevel,
            [In] IntPtr lpInputBuffer,
            ulong nInputBufferSize,
            [Out] IntPtr lpOutputBuffer,
            ulong nOutputBufferSize);

        /// <summary>
        /// Suspends the system by shutting power down. Depending on the Hibernate parameter,
        /// the system either enters a suspend (sleep) state or hibernation (S4).
        /// </summary>
        /// <param name="bHibernate">
        /// If this parameter is TRUE, the system hibernates.
        /// If the parameter is FALSE, the system is suspended.
        /// </param>
        /// <param name="bForce">
        /// Windows Server 2003, Windows XP, and Windows 2000:  If this parameter is TRUE,
        /// the system suspends operation immediately; if it is FALSE,
        /// the system broadcasts a PBT_APMQUERYSUSPEND event to each
        /// application to request permission to suspend operation.
        /// </param>
        /// <param name="bWakeupEventsDisabled">
        /// If this parameter is TRUE, the system disables all wake events.
        /// If the parameter is FALSE, any system wake events remain enabled.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is true.
        /// </returns>
        [DllImport("powrprof.dll", SetLastError = true)]
        public static extern bool SetSuspendState(
            bool bHibernate,
            bool bForce,
            bool bWakeupEventsDisabled);
    }
}
