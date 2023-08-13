using subtrack.DAL.Entities;

namespace subtrack.MAUI.Services.Abstractions
{
    public interface ISettingsService
    {
        Task<T?> GetByIdAsync<T>(string id) where T : SettingsBase;

        Task UpdateAsync<T>(T setting) where T : SettingsBase;
    }
}
