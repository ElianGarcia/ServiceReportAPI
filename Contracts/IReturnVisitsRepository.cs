using ServiceReportAPI.Models;

namespace ServiceReportAPI.Contracts
{
    public interface IReturnVisitsRepository
    {
        public Task<IEnumerable<ReturnVisit>> GetReturnVisits(long UserId);
        public Task<int> CreateReturnVisit(ReturnVisit ReturnVisits);
        public Task<int> UpdateReturnVisit(ReturnVisit ReturnVisits);
    }
}
