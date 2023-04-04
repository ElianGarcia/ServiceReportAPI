using ServiceReportAPI.Models;

namespace ServiceReportAPI.Contracts
{
    public interface ISettingsRepository
    {
        public Task<Settings> GetSettings(long UserId);
        public Task<int> UpdateSettings(Settings settings);
    }
}
