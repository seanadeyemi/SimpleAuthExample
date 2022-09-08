using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SimpleAuthExample.Application.Dtos;
using SimpleAuthExample.Application.Services.Interfaces;
using SimpleAuthExample.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAuthExample.Application.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserService> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, ILogger<UserService> logger, SignInManager<User> signInManager, RoleManager<Role> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<User> AuthenticateUser(string username, string password)
        {
            try
            {
                var existingUser = await GetUserIfExists(username);
                if (existingUser != null)
                {
                    existingUser = await CheckPassword(existingUser, password);
                }
                return existingUser;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error authenticating user: {ex.Message} {ex.StackTrace}");
                throw;
            }

        }

        public async Task<User> CheckPassword(User user, string password)
        {


            var result = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }
            await _signInManager.PasswordSignInAsync(user.Email, password, false, false);

            return user;
        }

        public async Task<(bool, string)> CreateUser(UserSignupDto userSignupDto)
        {
            //Check if role assigned to user exists
            var roleFound = await _roleManager.FindByNameAsync(userSignupDto.UserRole);

            if (roleFound == null)
            {
                _logger.LogError("Role with name {0} does not exist", userSignupDto.UserRole);
                return (false, $"UserRole \'{userSignupDto.UserRole}\' does not exist.");
            }

            var user = new User
            {
                Email = userSignupDto.Email,
                EmailConfirmed = true,
                NormalizedEmail = userSignupDto.Email.ToUpper(),
                NormalizedUserName = userSignupDto.Email.ToUpper(),
                UserName = userSignupDto.Email,
                PhoneNumber = userSignupDto.PhoneNumber,
                FirstName = userSignupDto.FirstName,
                LastName = userSignupDto.LastName
            };

            //Create user
            var result = await _userManager.CreateAsync(user, userSignupDto.Password);

            if (!result.Succeeded)
            {
                _logger.LogInformation("Something went wrong. Failed to create user.");
                return (false, "Something went wrong. Failed to create user.");
            }

            //assign user a role
            var userResult = await _userManager.AddToRoleAsync(user, userSignupDto.UserRole);

            // check if the user is assigned to the role successfully
            if (!userResult.Succeeded)
            {
                _logger.LogInformation($"Couldn't assign user a role");
                return (false, $"Couldn't assign user a role");
            }


            return (true, "User has been added successfully.");
        }

        public async Task<List<string>> GetRolesByUser(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<User> GetUserIfExists(string username)
        {
            User user = null;
            try
            {
                user = await _userManager.FindByEmailAsync(username);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public List<UserDto> GetUsers()
        {
            try
            {
                var users = _userManager.Users.ToList();
                return _mapper.Map<List<UserDto>>(users);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message} {ex.StackTrace}");
                throw;
            }
        }
    }
}
