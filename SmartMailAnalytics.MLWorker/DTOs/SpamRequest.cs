using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMailAnalytics.MLWorker.DTOs
{
    public class SpamRequest
    {
        public int MailId { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
