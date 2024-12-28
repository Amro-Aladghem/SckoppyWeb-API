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
    public class Token
    {
        [Key]
        public int TokenId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public string CreatedToken { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreationDateTime { get; set; }  
        public DateTime ? ExpierdDateTime { get;set; }
    }
}
