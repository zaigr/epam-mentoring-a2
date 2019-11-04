using System;
using System.Collections.Generic;
using System.Text;

namespace PowerManagement
{
    public interface ISuspendManagementProvider
    {
        void ReserveHibernationFile();

        void RemoveHibernationFile();

        void PutMachineHibernate();

        void PutMachineSleep();
    }
}
