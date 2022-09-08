using SimpleAuthExample.Application.Dtos;
using SimpleAuthExample.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAuthExample.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserIfExists(string username);
        Task<User> CheckPassword(User user, string password);
        Task<User> AuthenticateUser(string username, string password);
        Task<List<string>> GetRolesByUser(User user);
        Task<(bool, string)> CreateUser(UserSignupDto userSignupDto);
        List<UserDto> GetUsers();
    }
}
