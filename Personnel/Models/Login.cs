using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Personnel.Models
{
    public class Login
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Site { get; set; }

    }
}
