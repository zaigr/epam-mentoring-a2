using System.Runtime.InteropServices;

namespace PowerManagement.PowerManagement.Types
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SystemBatteryState
    {
        /// <summary>
        /// If this member is TRUE, the system battery charger is currently operating on external power.
        /// </summary>
        [MarshalAs(UnmanagedType.I1)]
        public bool AcOnLine;

        /// <summary>
        /// If this member is TRUE, at least one battery is present in the system.
        /// </summary>
        [MarshalAs(UnmanagedType.I1)]
        public bool BatteryPresent;

        /// <summary>
        /// If this member is TRUE, a battery is currently charging.
        /// </summary>
        [MarshalAs(UnmanagedType.I1)]
        public bool Charging;

        /// <summary>
        /// If this member is TRUE, a battery is currently discharging.
        /// </summary>
        [MarshalAs(UnmanagedType.I1)]
        public bool Discharging;

        /// <summary>
        /// Reserved.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
        public bool[] Spare1;

        /// <summary>
        /// Reserved.
        /// </summary>
        public byte Tag;

        /// <summary>
        /// The theoretical capacity of the battery when new.
        /// </summary>
        public uint MaxCapacity;

        /// <summary>
        /// The estimated remaining capacity of the battery.
        /// </summary>
        public uint RemainingCapacity;

        /// <summary>
        /// The current rate of discharge of the battery, in mW.
        /// </summary>
        public uint Rate;

        /// <summary>
        /// The estimated time remaining on the battery, in seconds.
        /// </summary>
        public uint EstimatedTime;

        public uint DefaultAlert1;

        public uint DefaultAlert2;
    }
}
