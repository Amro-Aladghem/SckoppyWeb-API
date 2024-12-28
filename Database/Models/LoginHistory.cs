using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public  class LoginHistory
    {
        [Key]
        public long LoginHisId { get;set; }

        [ForeignKey("User")]
        public int UserId { get;set; }
        public User User { get;set; }   
        public DateTime LoginDateTime { get; set; }
        public string ? IpAddress { get; set; }
        public string ? DeviceInfo { get; set; }
        public bool LoginStatus { get; set; }   
        public DateTime ? LogoutDateTime { get; set; }


    }
}
