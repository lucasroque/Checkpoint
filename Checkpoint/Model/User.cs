using System;

namespace Checkpoint.Model
{
    class User
    {
        public Int32 idUser { get; set; }
        public UserProfile userProfile { get; set; }
        public String login { get; set; }
        public String password { get; set; }
        public DateTime validity { get; set; }
        public Boolean active { get; set; }
    }
}
