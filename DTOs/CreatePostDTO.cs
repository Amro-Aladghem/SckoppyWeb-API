using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class CreatePostDTO
    {
        public string? Title { get; set; }
        public string? Body { get; set; }
        public int UserId { get; set; }
    }
}
