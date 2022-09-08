using AutoMapper;
using SimpleAuthExample.Api.Models;
using SimpleAuthExample.Application.Dtos;
using SimpleAuthExample.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleAuthExample.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            UserSignupMap();
            UserMap();
        }

        private void UserMap()
        {
            CreateMap<User, UserDto>();
        }

        private void UserSignupMap()
        {
            CreateMap<UserSignup, UserSignupDto>();
        }
    }
}
