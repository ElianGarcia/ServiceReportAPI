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
            (@Hours, @Videos, @Placements, @Date, @UserId); SELECT SCOPE_IDENTITY();";

            var parameters = new DynamicParameters();
            parameters.Add("ActivityId", Activity.ActivityId, DbType.Int64);
            parameters.Add("Hours", Activity.Hours, DbType.Decimal);
            parameters.Add("Videos", Activity.Videos, DbType.Int32);
            parameters.Add("Placements", Activity.Placements, DbType.Int32);
            //parameters.Add("ReturnVisits", Activity.ReturnVisits, DbType.Int32);
            parameters.Add("Date", Activity.Date, DbType.DateTime2);
            parameters.Add("UserId", Activity.UserId, DbType.Int64);

            //if (Activity.ReturnVisits > 0)
            //{
            //    query += @"INSERT INTO ReturnVisits 
            //    (Date, StudentId, UserId) 
            //    VALUES 
            //    (@Date, 0, @UserId);";
            //}

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryFirstAsync<int>(query, parameters);
                return result;
            }

        }

        public async Task<Activity> GetActivityByDate(long UserId, DateTime Date)
        {

            var query = @"SELECT 
                a.ActivityId,
                ISNULL(a.Hours, 0) AS Hours, 
                ISNULL(a.Placements, 0) AS Placements, 
                ISNULL(a.Videos, 0) AS Videos, 
                @DateReq AS Date,
	            ISNULL((SELECT COUNT(VisitId) FROM ReturnVisits WHERE CONVERT(varchar, a.Date, 111) = @DateReq AND UserId = @UserId), 0) AS ReturnVisits,
                a.UserId 
            FROM Activity a
            WHERE a.UserId = @UserId AND CONVERT(varchar, a.Date, 111) = @DateReq
            GROUP BY a.ActivityId, a.Hours, a.Placements, a.Videos, a.Date, a.UserId";

            var parameters = new DynamicParameters();
            parameters.Add("UserId", UserId, DbType.Int64);
            parameters.Add("DateReq", Date.ToString("yyyy/MM/dd"), DbType.String);

            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var result = await connection.QueryFirstAsync<Activity>(query, parameters);
                    return result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }
        }

        public async Task<Activity> GetTodaysActivity(long UserId)
        {
            var query = @"SELECT 
                a.ActivityId,
                ISNULL(a.Hours, 0) AS Hours, 
                ISNULL(a.Placements, 0) AS Placements, 
                ISNULL(a.Videos, 0) AS Videos, 
                ISNULL(a.Date, GETDATE()) AS Date,
	            ISNULL((SELECT COUNT(VisitId) FROM ReturnVisits WHERE FORMAT(a.Date,'dd/MM/yyyy') = FORMAT(GETDATE(),'dd/MM/yyyy') AND UserId = @UserId), 0) AS ReturnVisits,
                a.UserId 
            FROM Activity a LEFT JOIN ReturnVisits r ON a.UserId = r.UserId
            WHERE a.UserId = @UserId AND FORMAT(a.Date,'dd/MM/yyyy') = FORMAT(GETDATE(),'dd/MM/yyyy')
            GROUP BY a.ActivityId, a.Hours, a.Placements, a.Videos, a.Date, a.UserId, r.VisitId";

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

        public async Task<IEnumerable<Activity>> GetActualMonthActivity(long UserId)
        {
            var query = @"SELECT DISTINCT
                a.ActivityId,
                ISNULL(a.Hours, 0) AS Hours, 
                ISNULL(a.Placements, 0) AS Placements, 
                ISNULL(a.Videos, 0) AS Videos, 
                ISNULL(a.Date, GETDATE()) AS Date,
	            ISNULL((
		            SELECT COUNT(VisitId) FROM ReturnVisits WHERE FORMAT(a.Date,'dd/MM/yyyy') = FORMAT(Date, 'dd/MM/yyyy')), 0) AS ReturnVisits,
                a.UserId 
            FROM Activity a LEFT JOIN ReturnVisits r ON a.UserId = r.UserId
            WHERE a.UserId = @UserId AND MONTH(a.Date) = MONTH(GETDATE())
            GROUP BY a.ActivityId, a.Hours, a.Placements, a.Videos, a.Date, a.UserId, r.VisitId";

            var parameters = new DynamicParameters();
            parameters.Add("UserId", UserId, DbType.Int64);

            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var result = await connection.QueryAsync<Activity>(query, parameters);
                    return result;
                }
                catch (Exception e)
                {
                    return null;
                }

            }
        }

        public async Task<int> UpdateActivity(Activity Activity)
        {
            var query = @"UPDATE Activity
            SET Hours = @Hours,
                Videos = @Videos,
                Placements = @Placements
            WHERE ActivityId = @ActivityId; SELECT @ActivityId AS Result;";

            var parameters = new DynamicParameters();
            parameters.Add("ActivityId", Activity.ActivityId, DbType.Int64);
            parameters.Add("Hours", Activity.Hours, DbType.Decimal);
            parameters.Add("Videos", Activity.Videos, DbType.Int32);
            parameters.Add("Placements", Activity.Placements, DbType.Int32);
            //parameters.Add("ReturnVisits", Activity.ReturnVisits, DbType.Int32);
            parameters.Add("Date", Activity.Date.ToShortDateString(), DbType.DateTime2);
            parameters.Add("DateToCompare", Activity.Date.ToString("yyyy/MM/dd"), DbType.String);
            parameters.Add("UserId", Activity.UserId, DbType.Int64);

            //if (Activity.ReturnVisits > 0)
            //{
            //    //check quantity of return visits are already in the database
            //    var query2 = @"SELECT COUNT(VisitId) FROM ReturnVisits WHERE FORMAT(Date,'yyyy/MM/dd') = @DateToCompare AND UserId = @UserId AND StudentId = 0";

            //    using (var connection = _context.CreateConnection())
            //    {
            //        var result = await connection.ExecuteAsync(query2, parameters);
            //        var difference = Activity.ReturnVisits - result;

            //        if (difference > 0)
            //        {
            //            for (int i = 1; i < difference; i++)
            //            {
            //                query += @"INSERT INTO ReturnVisits 
            //            (Date, StudentId, UserId) 
            //            VALUES 
            //            (@Date, 0, @UserId);";
            //            }
            //        }
            //        else if (difference < 0)
            //        {
            //            //fix the counter as it cannot be less than 0
            //            for (int i = 0; i < Math.Abs(difference); i++)
            //            {
            //                query += @"DELETE FROM ReturnVisits WHERE CONVERT(varchar, Date, 111) = @Date AND UserId = @UserId AND StudentId = 0";
            //            }
            //        }
            //    }
            //}

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryFirstAsync<int>(query, parameters);
                return result;
            }
        }
    }
}
