using SimpleAuthExample.Application.Services.Interfaces;
using SimpleAuthExample.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAuthExample.Application.Services.Implementation
{
    public class TokenService : ITokenService

    {
        public string GenerateToken(User user, List<string> roles)
        {
            throw new NotImplementedException();
        }
    }
}
