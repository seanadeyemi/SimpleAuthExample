using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleAuthExample.Api.Models;
using SimpleAuthExample.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleAuthExample.Api.Controllers
{
    [Route("simple-auth")]
    [ApiController]
    public class UserController : ControllerBase
    {
       private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-users")]
        public  IActionResult GetUsers()
        {
            var users =  _userService.GetUsers();

            return Ok(new {Description = "List of Users" , Value = users });
        }

        [Authorize]
        [HttpGet("get-signed-in-user")]
        public IActionResult GetSignedInUser()
        {
            var currentUser = HttpContext.User;
            var firstName = currentUser.Claims.FirstOrDefault(c => c.Type == "firstName").Value;
            var lastName = currentUser.Claims.FirstOrDefault(c => c.Type == "lastName").Value;  
            
            var phone = currentUser.Claims.FirstOrDefault(c => c.Type == "phoneNumber").Value;

            var isAdmin = false;
            if (currentUser.HasClaim(c => c.Type == ClaimTypes.Role))
                isAdmin = true;

                return Ok(new { FirstName = firstName, LastName = lastName, Phone = phone, IsAdmin = isAdmin });
        }

    }
}
