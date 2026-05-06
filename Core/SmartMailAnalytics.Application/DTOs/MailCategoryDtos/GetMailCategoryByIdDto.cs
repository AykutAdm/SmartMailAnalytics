using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMailAnalytics.Application.DTOs.MailCategoryDtos
{
    public class GetMailCategoryByIdDto
    {
        public int MailCategoryId { get; set; }
        public string Name { get; set; }
    }
}
