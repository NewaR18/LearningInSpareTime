using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace TryPKI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : Controller
    {
        [HttpGet]
        [Route("[Action]")]
        public string Get()
        {
            return "No Certificates Needed Method";
        }
        [Authorize]
        [HttpPost]
        [Route("[Action]")]
        public string Post()
        {
            return "Certificate is validated and you are inside the method now";
        }
    }
}
