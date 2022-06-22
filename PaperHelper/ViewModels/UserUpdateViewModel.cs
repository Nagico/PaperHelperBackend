namespace PaperHelper.ViewModels;

/// <summary>
/// 用户信息更新模型
/// </summary>
public class UserUpdateViewModel
{
    public string? Username { get; set; }
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
    public string? Phone { get; set; }
}