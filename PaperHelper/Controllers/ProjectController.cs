using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PaperHelper.Controllers;

[ApiController]
[Route("projects")]
[Authorize]
public class ProjectController : ControllerBase
{
    //private readonly Project
}