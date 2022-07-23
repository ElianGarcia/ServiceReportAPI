using ServiceReportAPI.Models;

namespace ServiceReportAPI.Contracts
{
    public interface IUsersRepository
    {
        public Task<IEnumerable<User>> GetUsers();
        public Task<int> CreateUser(User User);
        public Task<int> UpdateUser(User User);
        public Task<int> DeleteUser(long UserId);
    }
}
