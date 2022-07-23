using Dapper;
using ServiceReportAPI.Context;
using ServiceReportAPI.Contracts;
using ServiceReportAPI.Models;
using System.Data;

namespace ServiceReportAPI.Repository
{
    public class PlacementsRepository : IPlacementsRepository
    {
        private readonly DapperContext _context;
        public PlacementsRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreatePlacement(Placement placement)
        {
            var query = "INSERT INTO Placements (Name, ShortName) VALUES (@Name, @ShortName)";
            var parameters = new DynamicParameters();
            parameters.Add("Name", placement.Name, DbType.String);
            parameters.Add("ShortName", placement.ShortName, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, parameters);
                return result;
            }
        }

        public async Task<Placement> GetPlacementById(long placementId)
        {
            var query = "SELECT * FROM Placements WHERE PlacementId = @PlacementId";
            var parameters = new DynamicParameters();
            parameters.Add("PlacementId", placementId, DbType.Int64);

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryFirstAsync<Placement>(query, parameters);
                return result;
            }
        }

        public async Task<IEnumerable<Placement>> GetPlacements()
        {
            var query = "SELECT * FROM Placements";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsync<Placement>(query, null);
                return result;
            }
        }
    }
}
