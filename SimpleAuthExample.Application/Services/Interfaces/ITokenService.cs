using SimpleAuthExample.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAuthExample.Application.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user, List<string> roles);
    }
}
