using System;

namespace Checkpoint.Model
{
    class UserProfile
    {
        public Int32 idUserProfile { get; set; }
        public String description { get; set; }
        public Int32 securityLevel { get; set; }

        public override string ToString()
        {
            return description;
        }
    }
}
