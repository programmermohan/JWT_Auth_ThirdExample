using JWT_Auth_ThirdExample.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_Auth_ThirdExample.Controllers
{
    [Authorize(Roles = AuthorizeRoles.Manager)]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        [HttpGet]
        [Route("GetEmployees")]
        public IActionResult GetEmployees()
        {
            return Ok(new List<string>()
            {
                "Employee01", "Employee02", "Employee03", "Employee04", "Employee05"
            });
        }
    }
}
