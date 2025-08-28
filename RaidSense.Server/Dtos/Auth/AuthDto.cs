using System.ComponentModel.DataAnnotations;

namespace RaidSense.Server.Dtos.Auth
{
    public class AuthDto
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
    }
}
