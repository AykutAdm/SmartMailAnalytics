using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMailAnalytics.Application.DTOs.UserDtos
{
    public class GetUserByIdDto
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
