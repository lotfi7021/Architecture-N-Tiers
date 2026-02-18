using System.ComponentModel.DataAnnotations;

namespace Projet.DTOs.Auth
{
    public class AuthResponse
    {
        [Required]
        public string Token { get; set; } = null!;
        [Required]
        public string Role { get; set; } = null!;
    }
}
