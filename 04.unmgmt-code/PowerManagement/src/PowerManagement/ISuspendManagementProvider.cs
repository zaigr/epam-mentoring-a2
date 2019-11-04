using System.Runtime.InteropServices;

namespace PowerManagement
{
    [ComVisible(true)]
    [Guid("C28BD742-6C92-4BA4-83AD-4ECB351D8EF2")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface ISuspendManagementProvider
    {
        void ReserveHibernationFile();

        void RemoveHibernationFile();

        void PutMachineHibernate();

        void PutMachineSleep();
    }
}
