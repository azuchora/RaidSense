using RaidSense.Server.Dtos.Servers;
using RaidSense.Server.Models;

namespace RaidSense.Server.Mappers
{
    public static class ServerMapper
    {
        public static ServerDto ToDto(this RustServer rustServer)
        {
            return new ServerDto
            {
                Id = rustServer.Id,
                Name = rustServer.Name,
                LastFetched = rustServer.LastFetched,
                MapId = rustServer.MapId,
            };
        }
    }
}
