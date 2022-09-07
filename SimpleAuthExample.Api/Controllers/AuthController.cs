using Microsoft.AspNetCore.Mvc;
using SimpleAuthExample.Api.Models;
using SimpleAuthExample.Application.Services.Interfaces;
using System.Threading.Tasks;

namespace SimpleAuthExample.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        public async Task<ActionResult> Login([FromBody] UserLogin userLogin)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            //check if a user exists with the provided username and password
          var user =  await _userService.AuthenticateUser(userLogin.Username, userLogin.Password);

           if(user is null)
            {
                return this.Problem("username or password is incorrect", statusCode: 400);
            }

            //get roles 
           var roles = await _userService.GetRolesByUser(user);

            var token = _tokenService.GenerateToken(user, roles);

            return Ok(new { Token = token });

        }


       
    }
}
