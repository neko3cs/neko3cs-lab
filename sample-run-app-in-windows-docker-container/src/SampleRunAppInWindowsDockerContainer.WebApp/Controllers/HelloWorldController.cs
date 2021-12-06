using Microsoft.AspNetCore.Mvc;

namespace SampleRunAppInWindowsDockerContainer.WebApp.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class HelloWorldController : ControllerBase
    {
        [HttpGet]
        public string Get() => "Hello World";

        [HttpGet("{name}")]
        public string Get(string name) => $"Hello {name}";
    }
}
