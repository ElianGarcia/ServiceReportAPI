using Dapper;
using ServiceReportAPI.Context;
using ServiceReportAPI.Contracts;
using ServiceReportAPI.Models;
using System.Data;

namespace ServiceReportAPI.Repository
{
    public class StudentsRepository : IStudentsRepository
    {
        private readonly DapperContext _context;

        public StudentsRepository(DapperContext repository)
        {
            _context = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<int> CreateStudent(Student student)
        {
            var query = @"INSERT INTO Students (Name, Address, Phone, 
                PlacementId, Active, DayToVisit, Observations, UserId) VALUES 
            (@Name, @Address, @Phone, @PlacementId, @Active, @DayToVisit, @Observations, @UserId)";

            var parameters = new DynamicParameters();
            parameters.Add("Name", student.Name, DbType.String);
            parameters.Add("Address", student.Address, DbType.String);
            parameters.Add("Phone", student.Phone, DbType.String);
            parameters.Add("PlacementId", student.PlacementId, DbType.Int64);
            parameters.Add("Active", true, DbType.Boolean);
            parameters.Add("DayToVisit", student.DayToVisit, DbType.Int16);
            parameters.Add("Observations", student.Observations, DbType.String);
            parameters.Add("UserId", student.UserId, DbType.Int64);

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, parameters);
                return result;
            }
        }

        public async Task<int> DeleteStudent(long StudentId)
        {
            var query = "UPDATE Students SET Active = 0 WHERE StudentId = @StudentId";
            var parameters = new DynamicParameters();
            parameters.Add("StudentId", StudentId, DbType.Int64);

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, parameters);
                return result;
            }
        }

        public async Task<IEnumerable<Student>> GetStudents(long UserId)
        {
            var query = "SELECT s.*, p.ShortName AS PlacementName FROM Students s INNER JOIN Placements p ON s.PlacementId = p.PlacementId WHERE UserId = @UserId";
            var parameters = new DynamicParameters();
            parameters.Add("UserId", UserId, DbType.Int64);

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsync<Student>(query, parameters);
                return result;
            }
        }

        public async Task<int> UpdateStudent(Student student)
        {
            var query = @"UPDATE students SET Name = @Name
                Address = @Address;
                Phone = @Phone;
                PlacementId = @PlacementId;
                Active = true;
                DayToVisit = @Day;
                Observations = @Observations
            WHERE StudentId = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", student.StudentId, DbType.Int64);
            parameters.Add("Name", student.Name, DbType.String);
            parameters.Add("Address", student.Address, DbType.String);
            parameters.Add("Phone", student.Phone, DbType.String);
            parameters.Add("PlacementId", student.PlacementId, DbType.Int64);
            parameters.Add("Active", true, DbType.Boolean);
            parameters.Add("DayToVisit", student.DayToVisit, DbType.Int16);
            parameters.Add("Observations", student.Observations, DbType.String);
            parameters.Add("UserId", student.UserId, DbType.Int64);

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, parameters);
                return result;
            }
        }
    }
}
