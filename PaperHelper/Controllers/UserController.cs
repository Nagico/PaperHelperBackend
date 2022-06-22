using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PaperHelper.Entities;
using PaperHelper.Exceptions;
using PaperHelper.Services;
using PaperHelper.ViewModels;

namespace PaperHelper.Controllers;

[ApiController]
[Route("users")]
[Authorize]
public class UserController : BaseController
{
    private readonly UserService _userService;
    private readonly ProjectService _projectService;
    
    public UserController(IConfiguration configuration, PaperHelperContext paperHelperContext)
    {
        _userService = new UserService(configuration, paperHelperContext);
        _projectService = new ProjectService(configuration, paperHelperContext);
    }
    
    /// <summary>
    /// 获取当前用户详情
    /// </summary>
    /// <returns>用户信息</returns>
    [HttpGet(Name = "GetUser")]
    public ActionResult GetUser()
    {
        var userDetail = _userService.GetUserFullDetail(UserId);
        return Ok(userDetail);
    }
    
    /// <summary>
    /// 获取指定ID用户信息
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns>用户信息</returns>
    [HttpGet("{id}", Name = "GetUserById")]
    public ActionResult GetUserById(int id)
    {
        var userDetail = UserId == id ? _userService.GetUserFullDetail(id) : _userService.GetUserPartDetail(id);
        return Ok(userDetail);
    }
    
    /// <summary>
    /// 修改用户信息
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <param name="updateViewModel">用户信息</param>
    /// <returns>用户详情</returns>
    [HttpPut("{id}", Name = "UpdateUser")]
    public ActionResult UpdateUser(int id, UserUpdateViewModel updateViewModel)
    {
        var userDetail = _userService.UpdateUser(id, updateViewModel.Username, updateViewModel.OldPassword,
            updateViewModel.NewPassword, updateViewModel.Phone, UserId);
        return Ok(userDetail);
    }
    
    /// <summary>
    /// 上传用户头像
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <param name="file">头像文件</param>
    /// <returns></returns>
    [HttpPost("{id}/avatars", Name = "UploadAvatar")]
    public ActionResult UploadAvatar(int id, IFormFile file)
    {
        var userDetail = _userService.UpdateUserAvatar(id, file, UserId);
        return Ok(userDetail);
    }
    
    /// <summary>
    /// 获取用户项目列表
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns>项目信息列表</returns>
    /// <exception cref="AppError">无权限</exception>
    [HttpGet("{id}/projects", Name = "UserGetUserProjects")]
    public ActionResult UserGetUserProjects(int id)
    {
        if (UserId != id)
        {
            throw new AppError("A0312");
        }
        var userProjects = _projectService.GetUserProjects(id);
        return Ok(userProjects);
    }
    
    /// <summary>
    /// 加入项目
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <param name="body">邀请码</param>
    /// <returns>项目详情</returns>
    /// <exception cref="AppError">无权限</exception>
    [HttpPost("{id}/projects", Name = "UserJoinProject")]
    public ActionResult UserJoinProject(int id, [FromBody] JObject body)
    {
        if (UserId != id)
        {
            throw new AppError("A0312");
        }
        var code = body.GetValue("invitation_code");
        if (code == null)
        {
            throw new AppError("A0312");
        }
        var res = _userService.JoinProject(id, code.ToString());

        return Ok(res);
    }
}