
using authorization_roles_token.ServiceContract;
using authorization_roles_token.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authorization_roles_token.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> Authonticate([FromBody] LoginViewModel loginViewModel)
        {
            var user = await _userService.Authenticate(loginViewModel);
            if (user == null)
                return BadRequest(new { message = "wrong UserName/Password" });
            return Ok(user);
        }
    }
}
