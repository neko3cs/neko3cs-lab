using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsxWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CsxWebAPI.Controllers
{
    [Route("api/csx")]
    [ApiController]
    public class CsxController : ControllerBase
    {
        [HttpGet("now")]
        public async Task<ActionResult<string>> GetNowTimeAsync()
            => await CsxManager.RunScriptAsync<string>("GetDateTimeNow");

        [HttpGet("login")]
        public async Task<ActionResult<bool>> CanLogin([FromQuery] string id, [FromQuery] string password)
            => await CsxManager.RunScriptAsync<bool>("Login", new string[] { id, password });
    }
}
