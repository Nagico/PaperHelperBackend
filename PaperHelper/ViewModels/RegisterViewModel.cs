using System.ComponentModel.DataAnnotations;

namespace PaperHelper.ViewModels;

/// <summary>
/// 注册模型
/// </summary>
public class RegisterViewModel
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Phone { get; set; }
}