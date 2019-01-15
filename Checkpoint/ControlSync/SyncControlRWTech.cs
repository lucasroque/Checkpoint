using System;
using System.Collections.Generic;
using Checkpoint.Model;

namespace Checkpoint.Control
{
    class SyncControlRWTech : ISyncControl
    {
        
        Comunication syncController;

        public SyncControlRWTech(){
            
        }

        public List<Marking> readMarkings()
        {
            throw new NotImplementedException();
        }

        public void readEmployer()
        {
            
        }

        public void readEmployess()
        {
            throw new NotImplementedException();
        }

        public void readFingerPrints()
        {
            throw new NotImplementedException();
        }

        public void sendEmployees()
        {
            throw new NotImplementedException();
        }

        public void sendEmployer()
        {
            throw new NotImplementedException();
        }

        public void sendFingerPrints()
        {
            throw new NotImplementedException();
        }
    }
}
