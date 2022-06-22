using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PaperHelper.Controllers;

[ApiController]
[Route("attachments")]
[Authorize]
public class AttachmentController : BaseController
{
    
}