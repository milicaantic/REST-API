using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [JsonIgnore]
 

        public string PasswordHash { get; set; }
        
        [JsonIgnore]

        public ICollection<UserProduct> UserProducts { get; set; }
    }
}
