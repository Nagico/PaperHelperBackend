using System.ComponentModel.DataAnnotations;

namespace PaperHelper.Dtos;

public class LoginDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}