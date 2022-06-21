using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace PaperHelper.Controllers;

public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// 登录用户的Id
    /// </summary>
    protected int UserId => int.Parse(User.Claims.First(i => i.Type == "UserId").Value);
}