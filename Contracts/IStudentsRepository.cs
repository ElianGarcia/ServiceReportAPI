using ServiceReportAPI.Models;

namespace ServiceReportAPI.Contracts
{
    public interface IStudentsRepository
    {
        public Task<IEnumerable<Student>> GetStudents(long UserId);
        public Task<int> CreateStudent(Student Student);
        public Task<int> UpdateStudent(Student Student);
        public Task<int> DeleteStudent(long StudentId);
    }
}
