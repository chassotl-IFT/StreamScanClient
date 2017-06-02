using System.ComponentModel.DataAnnotations;

namespace StreamScan.Models
{
    public class Login
    {
        public int Id { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Username { get; set; }
    }
}