using Dapper;
using ServiceReportAPI.Context;
using ServiceReportAPI.Contracts;
using ServiceReportAPI.Models;
using System.Data;

namespace ServiceReportAPI.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DapperContext _context;

        public UsersRepository(DapperContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<User> CreateUser(User user)
        {
            var query = @"INSERT INTO Users (Name, UserName, Password, CountryId,
                IsAdmin, CongregationId, LastLogin, CreatedDate) VALUES 
            (@Name, @UserName, @Password, 64, 0, @CongregationId, GETDATE(), GETDATE()); SELECT *, '' AS Password FROM Users WHERE Username = @Username;";

            var parameters = new DynamicParameters();
            parameters.Add("Name", user.Name, DbType.String);
            parameters.Add("UserName", user.UserName, DbType.String);
            parameters.Add("Password", user.Password, DbType.String);
            parameters.Add("CongregationId", user.CongregationId, DbType.Int64);

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<User>(query, parameters);
                return result;
            }
        }

        public async Task<int> DeleteUser(long UserId)
        {
            var query = "UPDATE Users SET Active = 0 WHERE userId = @userId";
            var parameters = new DynamicParameters();
            parameters.Add("userId", UserId, DbType.Int64);

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, parameters);
                return result;
            }
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var query = "SELECT * FROM users";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsync<User>(query, null);
                return result;
            }
        }

        public async Task<User> GetUser(User user)
        {
            var query = "SELECT *, '' as Password FROM users WHERE Username = @User AND Password = @Password";
            var parameters = new DynamicParameters();
            parameters.Add("User", user.UserName, DbType.String);
            parameters.Add("Password", user.Password, DbType.String);

            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var result = await connection.QueryFirstAsync<User>(query, parameters);
                    return result;
                }
            }
            catch (Exception)
            {
                return null;
                throw;
            }

        }

        public async Task<int> UpdateUser(User user)
        {
            var query = @"UPDATE users SET Name = @Name
                UserName = @UserName, 
                Password = @Password, 
                IsAdmin = @IsAdmin, 
                Congregation = @CongregationId, 
                Active = @Active
            WHERE UserId = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", user.UserId, DbType.String);
            parameters.Add("Name", user.Name, DbType.String);
            parameters.Add("UserName", user.UserName, DbType.String);
            parameters.Add("Password", user.Password, DbType.String);
            parameters.Add("IsAdmin", user.IsAdmin, DbType.String);
            parameters.Add("CongregacionId", user.CongregationId, DbType.Int64);
            parameters.Add("Active", true, DbType.Boolean);

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, parameters);
                return result;
            }
        }
    }
}
