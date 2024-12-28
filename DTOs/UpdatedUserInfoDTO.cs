using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class UpdatedUserInfoDTO
    {
        public int UserId { get; set; }
        public string Email { get;set; }
        public string ? ProfileImageLink { get;set; }
    }
}
