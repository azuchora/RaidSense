using RaidSense.Server.Dtos.RustMaps;
using RaidSense.Server.Models;

namespace RaidSense.Server.Mappers
{
    public static class RustMapsMapper
    {
        public static RustMap ToEntity(this RustMapsDataDto dto)
        {
            return new RustMap
            {
                Id = dto.Id,
                Seed = dto.Seed,
                Size = dto.Size,
                IsCustomMap = dto.IsCustomMap,
                IsStaging = dto.IsStaging,
                RawImageUrl = dto.RawImageUrl,
                ImageUrl = dto.ImageUrl,
                ThumbnailUrl = dto.ThumbnailUrl,
                Url = dto.Url,
            };
        }
    }
}
