using ServiceReportAPI.Models;

namespace ServiceReportAPI.Contracts
{
    public interface ICongregationsRepository
    {
        public Task<IEnumerable<Congregation>> GetCongregations();
        public Task CreateCongregation(Congregation congregation);
    }
}
