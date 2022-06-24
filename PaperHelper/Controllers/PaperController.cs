using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    private readonly NoteService _noteService;

    public PaperController(IConfiguration configuration, PaperHelperContext paperHelperContext)
    {
        _paperService = new PaperService(configuration, paperHelperContext);
        _noteService = new NoteService(configuration, paperHelperContext);
    }
    
    /// <summary>
    /// 获取论文详情
    /// </summary>
    /// <param name="id">论文ID</param>
    /// <returns>详细信息</returns>
    [HttpGet("{id}", Name = "GetPaperDetail")]
    public ActionResult GetPaperDetail(int id)
    {
        var paper = _paperService.GetPaperDetail(id, UserId);
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
    /// 删除论文
    /// </summary>
    /// <param name="id">论文ID</param>
    /// <returns>空</returns>
    [HttpDelete("{id}", Name = "DeletePaper")]
    public ActionResult DeletePaper(int id)
    {
        _paperService.DeletePaper(id, UserId);
        return NoContent();
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
    
    /// <summary>
    /// 获取论文思维导图
    /// </summary>
    /// <param name="id">论文ID</param>
    /// <returns>思维导图</returns>
    [HttpGet("{id}/mindmaps", Name = "GetPaperMindMap")]
    public ActionResult GetPaperMindMap(int id)
    {
        var mindMap = _noteService.GetCreateMindMap(id, UserId);
        return Ok(mindMap);
    }
    
    /// <summary>
    /// 更新思维导图
    /// </summary>
    /// <param name="id">论文ID</param>
    /// <param name="mindMap">更新JSON内容</param>
    /// <returns>思维导图</returns>
    [HttpPut("{id}/mindmaps", Name = "UpdatePaperMindMap")]
    public ActionResult UpdatePaperMindMap(int id, [FromBody] JObject mindMap)
    {
        _noteService.UpdateMindMap(id, mindMap, UserId);
        return Ok(new JObject {["id"] = id});
    }
    
    /// <summary>
    /// 获取论文批注
    /// </summary>
    /// <param name="id">论文ID</param>
    /// <returns>批注</returns>
    [HttpGet("{id}/annotations", Name = "GetPaperAnnotation")]
    public ActionResult GetPaperAnnotation(int id)
    {
        var annotation = _noteService.GetCreateAnnotation(id, UserId);
        return Ok(annotation);
    }
    
    /// <summary>
    /// 更新论文批注
    /// </summary>
    /// <param name="id">论文ID</param>
    /// <param name="annotation">更新批注内容</param>
    /// <returns>批注</returns>
    [HttpPut("{id}/annotations", Name = "UpdatePaperAnnotation")]
    public ActionResult UpdatePaperAnnotation(int id, [FromBody] JObject annotation)
    {
        _noteService.UpdateAnnotation(id, annotation["content"].ToString(), UserId);
        return Ok(new JObject {["id"] = id});
    }

}