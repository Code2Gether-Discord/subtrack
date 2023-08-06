using Microsoft.EntityFrameworkCore;
using subtrack.DAL;
using subtrack.DAL.Entities;
using subtrack.MAUI.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subtrack.MAUI.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly SubtrackDbContext _context;
        public async Task<T> GetByIdAsync<T>(string id) where T : SettingsBase
        {
            return await _context.Settings.OfType<T>().FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task UpdateAsync<T>(T setting) where T : SettingsBase

        {
            _context.Update(setting);

            await _context.SaveChangesAsync();
        }
    }
}
