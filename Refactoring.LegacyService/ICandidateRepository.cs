using System.Threading.Tasks;

namespace Refactoring.LegacyService {
    public interface ICandidateRepository {
        Task AddCandidate(Candidate candidate);
    }
}
