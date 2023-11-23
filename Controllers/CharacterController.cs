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
        private static List<Character> assassin = new List<Character>{
            new Character(),
            new Character{ Name = "Hola"}
        };

        [HttpGet("GetAll")] 
        public ActionResult<List<Character>> Get() {
            return Ok(assassin);
        }
        
        [HttpGet]
        public ActionResult<List<Character>> GetSingle() {
            return Ok(assassin[0]);
        }
    }
}