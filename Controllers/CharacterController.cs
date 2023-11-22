using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using web_api2.Models;

namespace web_api2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private static Character assassin = new Character();

        [HttpGet]
        public IActionResult Get() {
            return Ok(assassin);
        }
    }
}