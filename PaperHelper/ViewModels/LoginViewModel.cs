using System.ComponentModel.DataAnnotations;

namespace PaperHelper.ViewModels;

/// <summary>
/// 登录数据
/// </summary>
public class LoginViewModel
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}