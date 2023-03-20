using AutoMapper;
using Traki.Api.Contracts.Auth;
using Traki.Api.Models;

namespace Traki.Api.Mapping
{
    public class UserModelsMappingProfile : Profile
    {
        public UserModelsMappingProfile()
        {
            CreateMap<CreateUserRequest, User>();
        }
    }
}
