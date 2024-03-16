using System.Threading.Tasks;

namespace Refactoring.LegacyService;

public static class CandidateDataAccess {

    // For adding Candidate to the database, instead of doing this inside this static class here,
    // we delegate this responsibility to candidateRepository since CandidateDataAccess is a high level module,
    // and therefore should not be concerned with actual implementation of adding a candidate.
    // Also using repository makes code cleaner as all querying related things are stored inside the repository.
    public async static Task AddCandidate(ICandidateRepository candidateRepository, Candidate candidate) {
        await candidateRepository.AddCandidate(candidate);
    }
}
