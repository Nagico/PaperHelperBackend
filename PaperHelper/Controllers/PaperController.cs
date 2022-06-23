using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaperHelper.Entities;
using PaperHelper.Services;

namespace PaperHelper.Controllers;

/// <summary>
/// 论文管理接口
/// </summary>
[ApiController]
[Route("papers")]
[Authorize]
public class PaperController : BaseController
{
    private readonly PaperService _paperService;
    
    public PaperController(IConfiguration configuration, PaperHelperContext paperHelperContext)
    {
        _paperService = new PaperService(configuration, paperHelperContext);
    }
    
    /// <summary>
    /// 获取论文详情
    /// </summary>
    /// <param name="id">论文ID</param>
    /// <returns>详细信息</returns>
    [HttpGet("{id}", Name = "GetPaperDetail")]
    public ActionResult GetPaperDetail(int id)
    {
        var paper = _paperService.GetPaperDetail(id);
        return Ok(paper);
    }
    

}