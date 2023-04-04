using Dapper;
using ServiceReportAPI.Context;
using ServiceReportAPI.Contracts;
using ServiceReportAPI.Models;
using System.Data;

namespace ServiceReportAPI.Repository
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly DapperContext _context;

        public SettingsRepository(DapperContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Settings> GetSettings(long UserId)
        {
            var query = @"SELECT * 
            FROM Settings WHERE UserId = @UserId";

            var parameters = new DynamicParameters();
            parameters.Add("UserId", UserId, DbType.Int64);

            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var result = await connection.QueryFirstAsync<Settings>(query, parameters);
                    return result;
                }
                catch (Exception e)
                {
                    return null;
                }

            }
        }

        public async Task<int> UpdateSettings(Settings settings)
        {
            var query = @"UPDATE Settings
            SET IncrementValue = @Increment,
                Language = @Lang
            WHERE UserId = @UserId;";

            var parameters = new DynamicParameters();
            parameters.Add("Increment", settings.IncrementValue, DbType.Decimal);
            parameters.Add("Lang", settings.Language, DbType.String);
            parameters.Add("UserId", settings.UserId, DbType.Int64);

            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var result = await connection.QueryFirstAsync<int>(query, parameters);
                    return result;
                }
                catch (Exception e)
                {
                    return 0;
                }

            }
        }
    }
}
