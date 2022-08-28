using System.ComponentModel.DataAnnotations;

namespace SnackerBox.Dto;

public class AuthenticationDto
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}