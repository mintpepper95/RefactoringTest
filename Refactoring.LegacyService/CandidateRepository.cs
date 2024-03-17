using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Refactoring.LegacyService;

// Introduced repository pattern, so all the candidate query related stuff are in one place,
// instead of scattering around in the codebase.
public class CandidateRepository : ICandidateRepository {
    private readonly string _connectionString;

    // High level modules shouldn't rely on details of low level modules.
    // We use Options pattern to read config settings. This adheres to interface segregation,
    // as we don't need to update the IConfig in all the classes where it's injected if we ever change the config.
    public CandidateRepository(IOptions<DatabaseOptions> options) {
        _connectionString = options.Value.ApplicationDatabase;
    }

    // Writing to db is an I/O bound task,so we do it async.
    public async Task AddCandidate(Candidate candidate) {
        using (var connection = new SqlConnection(_connectionString)) {
            var command = new SqlCommand {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "uspAddCandidate"
            };

            var firstNameParameter = new SqlParameter("@Firstname", SqlDbType.VarChar, 50) { Value = candidate.Firstname };
            command.Parameters.Add(firstNameParameter);
            var surnameParameter = new SqlParameter("@Surname", SqlDbType.VarChar, 50) { Value = candidate.Surname };
            command.Parameters.Add(surnameParameter);
            var dateOfBirthParameter = new SqlParameter("@DateOfBirth", SqlDbType.DateTime) { Value = candidate.DateOfBirth };
            command.Parameters.Add(dateOfBirthParameter);
            var emailAddressParameter = new SqlParameter("@EmailAddress", SqlDbType.VarChar, 50) { Value = candidate.EmailAddress };
            command.Parameters.Add(emailAddressParameter);
            var requireCreditCheckParameter = new SqlParameter("@RequireCreditCheck", SqlDbType.Bit) { Value = candidate.RequireCreditCheck };
            command.Parameters.Add(requireCreditCheckParameter);
            var creditParameter = new SqlParameter("@Credit", SqlDbType.Int) { Value = candidate.Credit };
            command.Parameters.Add(creditParameter);
            var positionIdParameter = new SqlParameter("@PositionId", SqlDbType.Int) { Value = candidate.Position.Id };
            command.Parameters.Add(positionIdParameter);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
