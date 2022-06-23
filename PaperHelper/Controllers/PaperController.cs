using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PaperHelper.Entities;
using PaperHelper.Entities.Entities;
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
    private readonly TagService _tagService;
    
    public PaperController(IConfiguration configuration, PaperHelperContext paperHelperContext)
    {
        _paperService = new PaperService(configuration, paperHelperContext);
        _tagService = new TagService(configuration, paperHelperContext);
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
    
    /// <summary>
    /// 更新论文信息
    /// </summary>
    /// <param name="id">论文ID</param>
    /// <param name="paper">更新信息</param>
    /// <returns>论文详情</returns>
    [HttpPut("{id}", Name = "UpdatePaper")]
    public ActionResult UpdatePaper(int id, [FromBody] JObject paper)
    {
        var res = _paperService.UpdatePaper(id, paper, UserId);
        return Ok(res);
    }
    
    /// <summary>
    /// 添加论文标签
    /// </summary>
    /// <param name="id">论文ID</param>
    /// <param name="tag">标签信息</param>
    /// <returns>标签详情</returns>
    [HttpPost("{id}/tags", Name = "AddTag")]
    public ActionResult AddTag(int id, [FromBody] Tag tag)
    {
        var newTag = _paperService.AddPaperTag(id, tag.Name, UserId);
        
        return Created($"papers/{id}/tag/{newTag["id"]}", newTag);
    }
    
    /// <summary>
    /// 删除论文标签
    /// </summary>
    /// <param name="id">论文ID</param>
    /// <param name="tagId">标签ID</param>
    /// <returns>空</returns>
    [HttpDelete("{id}/tags/{tagId}", Name = "DeleteTag")]
    public ActionResult DeleteTag(int id, int tagId)
    {
        _paperService.DeletePaperTag(id, tagId, UserId);
        return NoContent();
    }

}