using subtrack.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subtrack.MAUI.Services.Abstractions
{
    public interface ISettingsService
    {
        Task<T> GetByIdAsync<T>(string id) where T : SettingsBase;

        Task UpdateAsync<T>(T setting) where T : SettingsBase;
    }
}
