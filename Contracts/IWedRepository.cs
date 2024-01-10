using ServiceReportAPI.Models;

namespace ServiceReportAPI.Contracts
{
    public interface IWedRepository
    {
        public Task<IEnumerable<Invitee>> GetInvitees();
        public Task<Invitee> GetInvitee(int id);
        public Task<int> SaveInvitee(Invitee invitee);
    }
}
