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

        private Int64 IsExistingActivity(DateTime date, Int64 UserId)
        {
            string v = date.ToString("yyyy/MM/dd");
            var query = $"SELECT ActivityId FROM Activity WHERE '{v}' = CONVERT(varchar, date, 111) AND UserId = {UserId}";

            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var result = connection.QueryFirst<Int64>(query);
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }
        }

        public async Task<int> CreateActivity(Activity Activity)
        {
            DateTime formatedDate = new DateTime(Activity.Date.Year, Activity.Date.Month, Activity.Date.Day, 0, 0, 0);
            Int64 ActivityId = IsExistingActivity(formatedDate, Activity.UserId);

            if(ActivityId > 0)
            {
                Activity.ActivityId = ActivityId;
                return await UpdateActivity(Activity);
            }

            string query = @"INSERT INTO Activity 
            (Hours, Videos, Placements, Date, UserId) 
            VALUES 
            (@Hours, @Videos, @Placements, @Date, @UserId); SELECT SCOPE_IDENTITY();";

            var parameters = new DynamicParameters();
            parameters.Add("ActivityId", Activity.ActivityId, DbType.Int64);
            parameters.Add("Hours", Activity.Hours, DbType.Decimal);
            parameters.Add("Videos", Activity.Videos, DbType.Int32);
            parameters.Add("Placements", Activity.Placements, DbType.Int32);
            parameters.Add("Date", formatedDate, DbType.DateTime2);
            parameters.Add("UserId", Activity.UserId, DbType.Int64);

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
            DateTime formatedDate = new DateTime(Activity.Date.Year, Activity.Date.Month, Activity.Date.Day, 0, 0, 0);
            Int64 ActivityId = IsExistingActivity(formatedDate, Activity.UserId);

            if (ActivityId == 0)
            {
                return await CreateActivity(Activity);
            }

            var query = @"UPDATE Activity
            SET Hours = @Hours,
                Videos = @Videos,
                Placements = @Placements
            WHERE ActivityId = @ActivityId AND UserId = @UserId; SELECT @ActivityId AS Result;";

            var parameters = new DynamicParameters();
            parameters.Add("ActivityId", Activity.ActivityId, DbType.Int64);
            parameters.Add("Hours", Activity.Hours, DbType.Decimal);
            parameters.Add("Videos", Activity.Videos, DbType.Int32);
            parameters.Add("Placements", Activity.Placements, DbType.Int32);
            parameters.Add("Date", Activity.Date.ToShortDateString(), DbType.DateTime2);
            parameters.Add("DateToCompare", Activity.Date.ToString("yyyy/MM/dd"), DbType.String);
            parameters.Add("UserId", Activity.UserId, DbType.Int64);

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryFirstAsync<int>(query, parameters);
                return result;
            }
        }
    }
}
