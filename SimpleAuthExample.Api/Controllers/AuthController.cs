using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleAuthExample.Api.Models;
using SimpleAuthExample.Application.Dtos;
using SimpleAuthExample.Application.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace SimpleAuthExample.Api.Controllers
{
    [Route("simple-auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AuthController(IUserService userService, ITokenService tokenService, IMapper mapper)
        {
            _userService = userService;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            //check if a user exists with the provided username and password
            var user = await _userService.AuthenticateUser(userLogin.Username, userLogin.Password);

            //if user credentials are not authentic return bad request with a message 
            if (user is null)
            {
                return this.Problem("username or password is incorrect", statusCode: 400);
            }

            //get roles 
            var roles = await _userService.GetRolesByUser(user);

            //generate a jwt token
            var token = _tokenService.GenerateToken(user, roles);

            //return token in the response
            return Ok(new { Token = token });

        }
       // [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] UserSignup model)
        {

            var userSignupDto = _mapper.Map<UserSignupDto>(model);
            var (success, message) = await _userService.CreateUser(userSignupDto);


            if (success)
                return Ok(new { response = message });
            else
                return BadRequest(new { response = message });

        }


    }
}
