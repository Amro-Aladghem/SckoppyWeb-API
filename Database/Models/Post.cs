using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public  class Post
    {
        public int PostId { get; set; }

        [MaxLength(100,ErrorMessage ="Post length must be less than 100 char")]
        public string ? Title { get; set; }

        [MaxLength(2000,ErrorMessage ="You have exceed the char limit (2000 ch)")]
        public string ? Body { get; set;}
        public string ? PostImageLink { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public int CommentCount { get; set; }
        public int LikesCount { get; set; }
        public bool IsUpdated { get; set; } 

        public DateTime ? LastUpdatedDate { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<PostTag> PostTags { get; set; }

        public ICollection<Comment> Comments { get; set; }

    }
}
