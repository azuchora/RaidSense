using RaidSense.Server.Dtos.Map;
using RaidSense.Server.Models;
using System.Drawing;
using System;
using RaidSense.Server.Dtos.RustMaps;

namespace RaidSense.Server.Mappers
{
    public static class MapMapper
    {
        public static MapDto ToDto(this RustMap map)
        {
            return new MapDto
            {
                Id = map.Id,
                Seed = map.Seed,
                Size = map.Size,
                IsCustomMap = map.IsCustomMap,
                IsStaging = map.IsStaging,
                RawImageUrl = map.RawImageUrl,
                ImageUrl = map.ImageUrl,
                ThumbnailUrl = map.ThumbnailUrl,
                Url = map.Url,
                IsPopulated = map.IsPopulated,
            };
        }

        public static void UpdateFromDto(this RustMap map, RustMapsDataDto dto)
        {
            map.Seed = dto.Seed;
            map.Size = dto.Size;
            map.IsCustomMap = dto.IsCustomMap;
            map.IsStaging = dto.IsStaging;
            map.RawImageUrl = dto.RawImageUrl;
            map.ImageUrl = dto.ImageUrl;
            map.ThumbnailUrl = dto.ThumbnailUrl;
            map.Url = dto.Url;
        }
    }
}
