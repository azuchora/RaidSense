using Microsoft.Identity.Client;
using RaidSense.Server.Dtos.BattleMetrics.Server;
using RaidSense.Server.Models;

namespace RaidSense.Server.Mappers
{
    public static class BattleMetricsMapper
    {
        public static BmServerDto? ToServerDto(this BmServerResponseDto response)
        {
            var serverAttributes = response?.Data?.Attributes;
            var serverDetails = serverAttributes?.Details;
            var mapDetails = serverDetails?.RustMaps;

            if (serverAttributes == null || serverDetails == null || mapDetails == null)
                return null;

            var mapId = !string.IsNullOrEmpty(mapDetails.Url)
                ? new Uri(mapDetails.Url).Segments.Last().TrimEnd('/')
                : null;

            return new BmServerDto
            {
                Id = serverAttributes.Id,
                Name = serverAttributes.Name,
                Ip = serverAttributes.Ip,
                Port = serverAttributes.Port,
                Players = serverAttributes.Players,
                MaxPlayers = serverAttributes.MaxPlayers,
                Seed = mapDetails.Seed,
                Size = mapDetails.Size,
                Url = mapDetails.Url,
                MapId = mapId,
            };
        }

        public static RustServer ToRustServerEntity(this BmServerDto bmServerDto)
        {
            return new RustServer
            {
                Id = bmServerDto.Id,
                Name = bmServerDto.Name,
                LastFetched = DateTime.UtcNow,
                MapId = bmServerDto.MapId,
            };
        }

        public static Map? ToMapEntity(this BmServerDto bmServerDto)
        {
            if (bmServerDto?.MapId == null)
                return null;

            return new Map 
            {
                Id = bmServerDto.MapId,
                Seed = (int) bmServerDto.Seed!,
                Size = (int) bmServerDto.Size!,
            };
        }
    }
}
