using Dapper;
using ServiceReportAPI.Context;
using ServiceReportAPI.Contracts;
using ServiceReportAPI.Models;
using System.Data;

namespace ServiceReportAPI.Repository
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly DapperContext _context;

        public ActivityRepository(DapperContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> CreateActivity(Activity Activity)
        {
            var query = @"INSERT INTO Activity 
            (Hours, Videos, Placements, Date, UserId) 
            VALUES 
            (@Hours, @Videos, @Placements, @Date, @UserId)";

            var parameters = new DynamicParameters();
            parameters.Add("ActivityId", Activity.ActivityId, DbType.Int64);
            parameters.Add("Hours", Activity.Hours, DbType.Decimal);
            parameters.Add("Videos", Activity.Videos, DbType.Int32);
            parameters.Add("Placements", Activity.Placements, DbType.Int32);
            parameters.Add("Date", Activity.Date, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, parameters);
                return result;
            }
        }

        public async Task<Activity> GetTodaysActivity(long UserId)
        {
            var query = @"SELECT 
                ActivityId,
                ISNULL(Hours, 0) AS Hours, 
                ISNULL(Placements, 0) AS Placements, 
                ISNULL(Videos, 0) AS Videos, 
                ISNULL(Date, GETDATE()) AS Date,
                UserId 
                FROM Activity 
                WHERE UserId = @UserId 
                AND FORMAT(Date,'dd/MM/yyyy') = FORMAT(GETDATE(),'dd/MM/yyyy')";

            var parameters = new DynamicParameters();
            parameters.Add("UserId", UserId, DbType.Int64);

            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var result = await connection.QueryFirstAsync<Activity>(query, parameters);
                    return result;
                }
                catch (Exception e)
                {
                    return new Activity();
                }

            }
        }

        public async Task<int> UpdateActivity(Activity Activity)
        {
            var query = @"UPDATE Activity
            SET Hours = @Hours,
                Videos = @Videos,
                Placements = @Placements
            WHERE ActivityId = @ActivityId";

            var parameters = new DynamicParameters();
            parameters.Add("ActivityId", Activity.ActivityId, DbType.Int64);
            parameters.Add("Hours", Activity.Hours, DbType.Decimal);
            parameters.Add("Videos", Activity.Videos, DbType.Int32);
            parameters.Add("Placements", Activity.Placements, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, parameters);
                return result;
            }
        }
    }
}
