using Dapper;
using ServiceReportAPI.Context;
using ServiceReportAPI.Contracts;
using ServiceReportAPI.Models;

namespace ServiceReportAPI.Repository
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly DapperContext _context;

        public CountriesRepository(DapperContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Country>> GetCountries()
        {
            var query = "SELECT * FROM Countries";

            using (var connection = _context.CreateConnection())
            {
                var countries = await connection.QueryAsync<Country>(query);
                return countries.ToList();
            }
        }
    }
}
