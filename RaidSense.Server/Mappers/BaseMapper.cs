using RaidSense.Server.Dtos.Bases;
using RaidSense.Server.Models;

namespace RaidSense.Server.Mappers
{
    public static class BaseMapper
    {
        public static Base ToEntity(this CreateBaseDto dto)
        {
            return new Base
            {
                Name = dto.Name,
                X = dto.X,
                Y = dto.Y,
            };
        }

        public static BaseDto ToDto(this Base entity)
        {
            return new BaseDto
            {
                Name = entity.Name,
                X = entity.X,
                Y = entity.Y,
            };
        }
    }
}
