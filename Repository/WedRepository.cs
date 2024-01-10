using Dapper;
using ServiceReportAPI.Context;
using ServiceReportAPI.Contracts;
using ServiceReportAPI.Models;
using System.Data;

namespace ServiceReportAPI.Repository
{
    public class WedRepository : IWedRepository
    {

        private readonly DapperContext _context;

        public WedRepository(DapperContext repository)
        {
            _context = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Invitee> GetInvitee(int id)
        {
            var query = "SELECT * FROM Invitee WHERE Activo = 1 AND ID = @ID";
            var parameters = new DynamicParameters();

            parameters.Add("ID", id, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryFirstAsync<Invitee>(query, parameters);
                return result;
            }
        }

        public async Task<IEnumerable<Invitee>> GetInvitees()
        {
            var query = "SELECT * FROM Invitee WHERE Activo = 1";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsync<Invitee>(query);
                return result;
            }
        }

        public async Task<int> SaveInvitee(Invitee invitee)
        {
            var query = @"INSERT INTO Invitee (ID, Descripcion, Invitados, Confirmados, InvitadoA, Confirmado, Activo)
            VALUES (@ID, @Descripcion, @Invitados, @Confirmados, @InvitadoA, @Confirmado, 1)";

            var parameters = new DynamicParameters();
            parameters.Add("ID", invitee.ID, DbType.Int32);
            parameters.Add("Descripcion", invitee.Descripcion, DbType.String);
            parameters.Add("Invitados", invitee.Invitados, DbType.Int32);
            parameters.Add("Confirmados", invitee.Confirmados, DbType.Int32);
            parameters.Add("InvitadoA", invitee.InvitadoA, DbType.Int32);
            parameters.Add("Confirmado", invitee.Confirmado, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, parameters);
                return result;
            }
        }
    }
}
