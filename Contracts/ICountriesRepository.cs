using ServiceReportAPI.Models;

namespace ServiceReportAPI.Contracts
{
    public interface ICountriesRepository
    {
        public Task<IEnumerable<Country>> GetCountries();
    }
}
