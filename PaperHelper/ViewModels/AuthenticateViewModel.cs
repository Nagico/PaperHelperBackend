namespace PaperHelper.ViewModels;

/// <summary>
/// 登录返回数据
/// </summary>
public class AuthenticateViewModel
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Phone { get; set; }
    public Uri Avatar { get; set; }
    public string Token { get; set; }
}