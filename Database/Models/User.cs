using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Database.Models
{
    public  class User
    {
        [Key]
        public int UserId { get; set;}

        [Required]
        [MaxLength(100,ErrorMessage ="Max lenght is 100 char ")]
        public  string Username { get; set; }

        [Required]
        [MaxLength(100,ErrorMessage = "Max lenght is 100 char ")]
        public string Email { get; set; }

        [Required]
        [MinLength(8,ErrorMessage ="Password must be longer than or equale 8 char")]
        public string Password { get; set; }

        public string ? ProfileImageLink { get; set; }
        
        public int CommentsCount { get; set; }
        public int LikesCount { get; set; }
        public int PostCount { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public string ? LastUsedToken { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Post> Posts  { get; set; }
        

    }
}
