using System;
using Microsoft.AspNetCore.Mvc;

namespace MeuServicoApi.Controllers;

    [ApiController]
    [Route("[controller]")]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHello()
        {
            return Ok(new { message = "Olá do MeuServicoApi!" });
        }
    }