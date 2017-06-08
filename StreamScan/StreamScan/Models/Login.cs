using System.ComponentModel.DataAnnotations;

namespace StreamScan.Models
{
    /// <summary>
    /// Permet le stockage des identifiants d'un utilisateur lors de sa connexion
    /// </summary>
    public class Login
    {
        public int Id { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Username { get; set; }
    }
}