using System;
using System.Threading.Tasks;

namespace Refactoring.LegacyService;

public class CandidateService {
    private ICandidateFactory _candidateFactory;
    private ICandidateRepository _candidateRepository;

    public CandidateService(ICandidateFactory candidateFactory, ICandidateRepository candidateRepository) {
        _candidateFactory = candidateFactory;
        _candidateRepository = candidateRepository;
    }

    // Following the single responsibility principle, we have removed logics unrelated to adding Candidate into other classes.
    // CandidateService should not be responsible for things like fetching from PositionRepository, creation of Candidate,
    // parameter checking and updating Candidate instance.
    // It should only be concerned with logic for deciding whether an Candidate should be added to the database.
    // For creation of Candidate, we have introduced a factory class for handling what type of Candidate class to create based on positionid.
    // For adding Candidate to the database, we delegate this responsibility to candidateRepository,
    // instead of having query related code inside CandidateDataAccess.
    public async Task<bool> AddCandidate(string firstname, string surname, string email, DateTime dateOfBirth, int positionid) {
        var candidate = await _candidateFactory.CreateCandidate(positionid, dateOfBirth, email, firstname, surname);
        if (candidate.RequireCreditCheck && candidate.Credit < 500) {
            return false;
        }
        await CandidateDataAccess.AddCandidate(_candidateRepository, candidate);
        return true;
    }
}
