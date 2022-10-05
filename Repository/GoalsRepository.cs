using Dapper;
using ServiceReportAPI.Context;
using ServiceReportAPI.Contracts;
using ServiceReportAPI.Models;
using System.Data;

namespace ServiceReportAPI.Repository
{
    public class GoalsRepository : IGoalsRepository
    {
        private readonly DapperContext _context;

        public GoalsRepository(DapperContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> CreateGoal(Goal goal)
        {
            var query = "INSERT INTO Goals (Hours, Placements, Videos, UserId) VALUES (@Hours, @Placements, @Videos, @UserId)";
            var parameters = new DynamicParameters();
            parameters.Add("Hours", goal.Hours, DbType.Decimal);
            parameters.Add("Placements", goal.Placements, DbType.Int32);
            parameters.Add("Videos", goal.Videos, DbType.Int32);
            parameters.Add("UserId", goal.UserId, DbType.Int64);

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, parameters);
                return result;
            }
        }

        public async Task<Goal> GetGoal(long UserId)
        {
            var query = "SELECT * FROM Goals WHERE UserId = @UserId";
            var parameters = new DynamicParameters();
            parameters.Add("UserId", UserId, DbType.Int64);

            using (var connection = _context.CreateConnection())
            {
                var goal = await connection.QueryFirstAsync<Goal>(query, parameters);
                goal.Progress = this.GetProgress(UserId).Result.Last<Goal>();
                return goal;
            }
        }
        
        public async Task<IEnumerable<Goal>> GetProgress(long UserId)
        {
            var query = @"SELECT
                SUM(ISNULL(a.Hours, 0)) AS Hours, 
                SUM(ISNULL(a.Placements, 0)) AS Placements, 
	            SUM(ISNULL(a.Videos, 0)) AS Videos,
	            ISNULL((SELECT COUNT(VisitId) FROM ReturnVisits WHERE MONTH(Date) = MONTH(a.Date) AND UserId = @UserId), 0) AS ReturnVisits, 
	            MONTH(a.Date) AS Month
            FROM Activity a
            WHERE a.UserId = @UserId
            GROUP BY MONTH(a.Date)
            ORDER BY MONTH(a.Date)";

            var parameters = new DynamicParameters();
            parameters.Add("UserId", UserId, DbType.Int64);

            using (var connection = _context.CreateConnection())
            {
                var goal = await connection.QueryAsync<Goal>(query, parameters);
                return goal;
            }
        }

        public async Task<int> UpdateGoal(Goal goal)
        {
            var query = "UPDATE Goals SET Hours = @Hours, Placements = @Placements, Videos = @Videos WHERE UserId = @UserId";
            var parameters = new DynamicParameters();
            parameters.Add("Hours", goal.Hours, DbType.Decimal);
            parameters.Add("Placements", goal.Placements, DbType.Int32);
            parameters.Add("Videos", goal.Videos, DbType.Int32);
            parameters.Add("UserId", goal.UserId, DbType.Int64);

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, parameters);
                return result;
            }
        }
    }
}
