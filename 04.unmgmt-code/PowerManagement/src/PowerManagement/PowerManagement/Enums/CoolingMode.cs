namespace PowerManagement.PowerManagement.Enums
{
    public enum CoolingMode
    {
        /// <summary>
        /// The system is currently in Active cooling mode.
        /// </summary>
        PO_TZ_ACTIVE = 0,

        /// <summary>
        /// The system is currently in Passive cooling mode.
        /// </summary>
        PO_TZ_PASSIVE = 1,

        /// <summary>
        /// The system does not support CPU throttling, or there is no thermal zone defined in the system.
        /// </summary>
        PO_TZ_INVALID_MODE = 2
    }
}
