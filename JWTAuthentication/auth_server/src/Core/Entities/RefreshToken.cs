using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class RefreshToken : BaseEntity<int>
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public DateTime ExpireAt { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
