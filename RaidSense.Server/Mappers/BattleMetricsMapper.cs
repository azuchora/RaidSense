using RaidSense.Server.Dtos.BattleMetrics.Server;

namespace RaidSense.Server.Mappers
{
    public static class BattleMetricsMapper
    {
        public static BmServerDto ToServerDto(this BmServerResponse response)
        {
            var mapDetails = response.Data.Attributes.Details.RustMaps;
            var serverAttributes = response.Data.Attributes;
            var serverDetails = serverAttributes.Details;
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
            };
        }
    }
}
