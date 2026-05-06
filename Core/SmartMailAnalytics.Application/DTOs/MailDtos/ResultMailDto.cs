using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMailAnalytics.Application.DTOs.MailDtos
{
    public class ResultMailDto
    {
        public int MailId { get; set; }
        public int UserId { get; set; }
        public int MailCategoryId { get; set; }
        public string SenderEmail { get; set; }
        public string ReceiverEmail { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public bool IsSpam { get; set; } // ML
        public DateTime CreatedDate { get; set; }
    }
}
