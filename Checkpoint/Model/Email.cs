using System;
using System.Collections.Generic;

namespace Checkpoint.Model
{
    class Email
    {
        public String sender { get; set; }
        public String receiver { get; set; }
        public String subject { get; set; }
        public String content { get; set; }
        public List<String> attachments { get; set; }

        public Email() {
            attachments = new List<String>();
        }

    }
}
