using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMailAnalytics.Application.DTOs.RabbitMqDtos
{
    public class SpamResponseDto
    {
        public int MailId { get; set; }
        public bool IsSpam { get; set; }
    }
}
