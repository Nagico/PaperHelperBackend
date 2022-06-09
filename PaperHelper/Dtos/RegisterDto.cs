using System.ComponentModel.DataAnnotations;

namespace PaperHelper.Dtos;

public class RegisterDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string ConfirmPassword { get; set; }
    [Required]
    public string Phone { get; set; }
}