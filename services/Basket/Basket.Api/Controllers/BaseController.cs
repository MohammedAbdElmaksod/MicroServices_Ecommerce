using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers
{
    
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
    }
}
