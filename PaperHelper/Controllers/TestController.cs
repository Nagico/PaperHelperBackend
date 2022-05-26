using Microsoft.AspNetCore.Mvc;
using PaperHelper.Entities;

namespace PaperHelper.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    private readonly PaperHelperContext _context;
    
    public TestController(PaperHelperContext context)
    {
        _context = context;
    }
    
    [HttpGet(Name = "Test")]
    public JsonResult Test()
    {
        return new JsonResult(new {
            time = DateTime.Now,
            message = "success"
        });
    }
}