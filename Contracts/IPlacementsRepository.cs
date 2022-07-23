using ServiceReportAPI.Models;

namespace ServiceReportAPI.Contracts
{
    public interface IPlacementsRepository
    {
        public Task<IEnumerable<Placement>> GetPlacements();
        public Task<int> CreatePlacement(Placement placement);
        public Task<Placement> GetPlacementById(Int64 placementId);
    }
}
