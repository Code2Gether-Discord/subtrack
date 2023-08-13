using Microsoft.EntityFrameworkCore;
using subtrack.DAL;
using subtrack.DAL.Entities;
using subtrack.MAUI.Services.Abstractions;

namespace subtrack.MAUI.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly SubtrackDbContext _dbContext;

        public SettingsService(SubtrackDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T?> GetByIdAsync<T>(string id) where T : SettingsBase
        {
            return await _dbContext.Settings.OfType<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync<T>(T setting) where T : SettingsBase

        {
            _dbContext.Update(setting);

            await _dbContext.SaveChangesAsync();
        }
    }
}
