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
public class PaperController
{
    private readonly PaperService _paperService;
    
    public PaperController(IConfiguration configuration, PaperHelperContext paperHelperContext)
    {
        _paperService = new PaperService(configuration, paperHelperContext);
    }


}