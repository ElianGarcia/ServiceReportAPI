using ServiceReportAPI.Models;

namespace ServiceReportAPI.Contracts
{
    public interface IActivityRepository
    {
        public Task<Activity> GetTodaysActivity(long UserId);
        public Task<Activity> GetActivityByDate(long UserId, DateTime Date);
        public Task<int> CreateActivity(Activity activity);
        public Task<int> UpdateActivity(Activity activity);
    }
}
