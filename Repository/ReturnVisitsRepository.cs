using Dapper;
using ServiceReportAPI.Context;
using ServiceReportAPI.Contracts;
using ServiceReportAPI.Models;
using System.Data;

namespace ServiceReportAPI.Repository
{
    public class ReturnVisitsRepository : IReturnVisitsRepository
    {
        private readonly DapperContext _context;

        public ReturnVisitsRepository(DapperContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> CreateReturnVisit(ReturnVisit visit)
        {
            var query = @"INSERT INTO ReturnVisits 
            (StudentId, Date, Notes, Active, UserId) 
            VALUES 
            (@StudentId, @Date, @Notes, 1, @UserId)";

            var parameters = new DynamicParameters();
            parameters.Add("StudentId", visit.StudentId, DbType.Int64);
            parameters.Add("UserId", visit.UserId, DbType.Int64);
            parameters.Add("Notes", visit.Notes, DbType.String);
            parameters.Add("Date", visit.Date, DbType.DateTime2);

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, parameters);
                return result;
            }
        }

        public async Task<IEnumerable<ReturnVisit>> GetReturnVisits(long StudentId)
        {
            var query = @"SELECT TOP 10 
                StudentId, Date, Notes, UserId
                FROM ReturnVisits 
                WHERE StudentId = @StudentId 
                AND Active = 1
                ORDER BY Date DESC";

            var parameters = new DynamicParameters();
            parameters.Add("StudentId", StudentId, DbType.Int64);

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsync<ReturnVisit>(query, parameters);
                return result;
            }
        }

        public async Task<int> UpdateReturnVisit(ReturnVisit visit)
        {
            var query = @"UPDATE ReturnVisits
            SET StudentId = @StudentId,
                Notes = @Notes,
                Date = @Date
            WHERE VisitId = @VisitId";

            var parameters = new DynamicParameters();
            parameters.Add("VisitId", visit.VisitId, DbType.Int64);
            parameters.Add("StudentId", visit.StudentId, DbType.Int64);
            parameters.Add("UserId", visit.UserId, DbType.Int64);
            parameters.Add("Notes", visit.Notes, DbType.String);
            parameters.Add("Date", visit.Date, DbType.DateTime2);

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, parameters);
                return result;
            }
        }
    }
}
