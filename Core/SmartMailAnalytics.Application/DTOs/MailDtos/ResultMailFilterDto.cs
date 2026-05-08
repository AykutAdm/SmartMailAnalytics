using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMailAnalytics.Application.DTOs.MailDtos
{
    public class ResultMailFilterDto
    {
        public string? SenderEmail { get; set; }
        public string? Subject { get; set; }
        public bool? IsSpam { get; set; }
        public int Page { get; set; } = 1;
    }
}
