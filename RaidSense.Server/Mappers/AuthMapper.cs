using RaidSense.Server.Dtos.Auth;
using RaidSense.Server.Models;

namespace RaidSense.Server.Mappers
{
    public static class AuthMapper
    {
        public static AuthDto ToAuthDto(this User user, string token)
        {
            return new AuthDto
            {
                Id = user.Id,
                Username = user.UserName!,
                Email = user.Email!,
                Token = token
            };
        }
    }
}
