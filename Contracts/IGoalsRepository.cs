using ServiceReportAPI.Models;

namespace ServiceReportAPI.Contracts
{
    public interface IGoalsRepository
    {
        public Task<Goal> GetGoal(long UserId);
        public Task<IEnumerable<Goal>> GetProgress(long UserId);
        public Task<int> CreateGoal(Goal goal);
        public Task<int> UpdateGoal(Goal goal);
    }
}
