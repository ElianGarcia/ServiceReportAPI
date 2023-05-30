using Dapper;
using ServiceReportAPI.Context;
using ServiceReportAPI.Contracts;
using ServiceReportAPI.Models;
using System.Data;

namespace ServiceReportAPI.Repository
{
    public class CongregationsRepository : ICongregationsRepository
    {
        private readonly DapperContext _context;
        public CongregationsRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task CreateCongregation(Congregation congregation)
        {
            var query = "INSERT INTO Congregations (Name, Code) VALUES (@Name, @Code)";
            var parameters = new DynamicParameters();
            parameters.Add("Name", congregation.Name, DbType.String);
            parameters.Add("Address", congregation.Code, DbType.Int64);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<IEnumerable<Congregation>> GetCongregations()
        {
            var query = "SELECT * FROM Congregations ORDER BY Name";

            using (var connection = _context.CreateConnection())
            {
                var congregations = await connection.QueryAsync<Congregation>(query);
                return congregations.ToList();
            }
        }
    }
}
