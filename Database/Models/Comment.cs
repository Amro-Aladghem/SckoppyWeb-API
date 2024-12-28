using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public  class Comment
    {
        public int CommentId { get;set; }

        [Required]
        [MaxLength(550,ErrorMessage ="The Comment length must be less than 550 char")]
        public string Body { get; set; }

        public DateTime CreateAt { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Post")]
        public int PostId { get;set; }
        public Post Post { get; set; }



    }
}
