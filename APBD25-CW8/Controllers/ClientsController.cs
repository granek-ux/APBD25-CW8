using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APBD25_CW8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("200");
        }
    }
}
