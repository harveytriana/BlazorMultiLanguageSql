using Microsoft.AspNetCore.Mvc;

namespace BlazorMultilanguage.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public string Get() => "Start Server";
    }
}
