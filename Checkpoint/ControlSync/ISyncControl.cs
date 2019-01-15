using Checkpoint.Model;
using System.Collections.Generic;

namespace Checkpoint.Control
{
    interface ISyncControl
    {
        List<Marking> readMarkings();

        void readEmployer();

        void readEmployess();

        void readFingerPrints();

        void sendEmployer();

        void sendEmployees();

        void sendFingerPrints();

    }
}
