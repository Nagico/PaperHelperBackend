using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace PaperHelper.Controllers;

public abstract class BaseController : ControllerBase
{
    protected int UserId => int.Parse(User.Claims.First(i => i.Type == "UserId").Value);
}