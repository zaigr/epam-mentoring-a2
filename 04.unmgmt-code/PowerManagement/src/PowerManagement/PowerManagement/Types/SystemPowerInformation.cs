using System.Runtime.InteropServices;

namespace PowerManagement.PowerManagement.Types
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SystemPowerInformation
    {
        /// <summary>
        /// The idleness at which the system is considered idle and the idle time-out begins counting,
        /// expressed as a percentage. Dropping below this number causes the timer to be canceled.
        /// </summary>
        public uint MaxIdlenessAllowed;

        /// <summary>
        /// The current idle level, expressed as a percentage.
        /// </summary>
        public uint Idleness;

        /// <summary>
        /// The time remaining in the idle timer, in seconds.
        /// </summary>
        public uint TimeRemaining;

        /// <summary>
        /// The current system cooling mode. This member must one of the following values.
        /// </summary>
        public byte CoolingMode;
    }
}
