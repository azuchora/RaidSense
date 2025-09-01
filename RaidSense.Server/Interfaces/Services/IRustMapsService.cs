using RaidSense.Server.Dtos.RustMaps;

namespace RaidSense.Server.Interfaces.Services
{
    public interface IRustMapsService
    {
        Task<RustMapsDataDto?> GetRustMapDetailsAsync(string mapId);
    }
}
