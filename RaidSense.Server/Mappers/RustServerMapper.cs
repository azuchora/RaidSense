using RaidSense.Server.Dtos.RustServer;
using RaidSense.Server.Models;

namespace RaidSense.Server.Mappers
{
    public static class RustServerMapper
    {
        public static RustServerDto ToDto(this RustServer rustServer)
        {
            return new RustServerDto
            {
                Id = rustServer.Id,
                Name = rustServer.Name,
                LastFetched = rustServer.LastFetched,
            };
        }
    }
}
