using Microsoft.EntityFrameworkCore;
using RaidSense.Server.Interfaces.Repositories;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Models;

namespace RaidSense.Server.Services
{
    public class BaseService : IBaseService
    {
        private readonly IBaseRepository _baseRepo;
        public BaseService(IBaseRepository baseRepo)
        {
            _baseRepo = baseRepo;
        }

        public async Task<Base> CreateAsync(Base newBase)
        {
            await _baseRepo.AddAndSaveAsync(newBase);
            return newBase;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _baseRepo.DeleteByIdAsync(id);
        }

        public async Task<Base?> GetByIdAsync(int id)
        {
            return await _baseRepo.GetByIdAsync(id);
        }

        public async Task<Base?> GetByIdDetailedAsync(int id)
        {
            var query = _baseRepo.GetQueryable();
            
            var foundBase = await query
                .Include(b => b.BasePlayers)
                .Include(b => b.Photos)
                .SingleOrDefaultAsync(b => b.Id == id);

            return foundBase;
        }

        public async Task UpdateAsync(Base newBase)
        {
            await _baseRepo.UpdateAsync(newBase);
        }
    }
}
