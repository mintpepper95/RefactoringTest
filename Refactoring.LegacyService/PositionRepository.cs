using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Refactoring.LegacyService;

// Introduced repository pattern, so all the position query related stuff are in one place,
// instead of scattering around in the codebase.
public class PositionRepository : IPositionRepository {
    private string _connectionString;

    // Since high level modules shouldn't rely on details of low level modules.
    // We introduced IConfigurationManagerWrapper, a wrapper that gets the connection string,
    // instead of retrieving directly from ConfigurationManager.
    public PositionRepository(IConfigurationManagerWrapper config) {
        _connectionString = config.GetConnectionString();
    }

    // Reading from db is an I/O bound task,so we do it async.
    public virtual async Task<Position> GetByIdAsync(int id) {
        Position position = null;

        using (var connection = new SqlConnection(_connectionString)) {
            await connection.OpenAsync();

            var command = new SqlCommand {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "uspGetPositionById"
            };

            command.Parameters.Add(new SqlParameter("@positionId", SqlDbType.Int) { Value = id });

            var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            // Instead of keep reading and overwriting position variable like in the original code,
            // We retrieve the value and store it into position variable and return immediately.
            // I'm making the assumption positionid is unique and that we won't have dupe data.
            if (await reader.ReadAsync()) {
                int positionId;
                // Check column for null and use try parse to avoid explicit exception handling
                var positionIdOrdinalIndex = reader.GetOrdinal("positionId");
                if (!reader.IsDBNull(positionIdOrdinalIndex) &&
                    int.TryParse(reader[positionIdOrdinalIndex].ToString(), out positionId)) {
                    position = new Position(
                        positionId,
                        reader["Name"].ToString(),
                        // Since Status is only read here, I updated Status property in Position to string type
                        reader["Status"].ToString());
                }
            }
            return position;
        }
    }
}
