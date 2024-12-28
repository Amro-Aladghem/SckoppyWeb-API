using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class UserDTO
    { 
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? ProfileImageLink { get; set; }
        public int CommentsCount { get; set; }
        public int PostCount { get; set; }
    }
}
