using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class PostDTO
    {
        public int PostId { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public UserDTO author { get; set; }
        public string? PostImageLink { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CommentCount { get; set; }   
        public int LikesCount { get; set; }
        public DateTime? LastUpdatedDateTime { get; set; }
        public ICollection<TagDTO> Tags { get; set; }
        public ICollection<CommentDTO> Comments { get; set; }

    }
}
