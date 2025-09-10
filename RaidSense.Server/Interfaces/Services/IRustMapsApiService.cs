using RaidSense.Server.Dtos.RustMaps;

namespace RaidSense.Server.Interfaces.Services
{
    public interface IRustMapsApiService
    {
        Task<RustMapsDataDto?> GetRustMapDetailsAsync(string mapId);
    }
}
