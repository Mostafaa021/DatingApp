using Microsoft.AspNetCore.Mvc;
using API.Filters;

namespace API.Controllers
{
    [ServiceFilter(typeof(LogUserActivityFilter))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        
    }
}