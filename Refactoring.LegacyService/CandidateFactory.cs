using System;
using System.Threading.Tasks;

namespace Refactoring.LegacyService;

public class CandidateFactory : ICandidateFactory {
    private ICandidateCreditService _candidateCreditService;
    private IPositionRepository _positionRepository;
    private ITimeProvider _timeProvider;

    // DI for loose coupling
    public CandidateFactory(ICandidateCreditService candidateCreditService, IPositionRepository positionRepository, ITimeProvider timeProvider) {
        _candidateCreditService = candidateCreditService;
        _positionRepository = positionRepository;
        _timeProvider = timeProvider;
    }

    // For creation of Candidate, we have introduced a factory class for handling what type of Candidate class to create based on positionid.
    // Also async as it's I/O bound task ( as we read credit from db and remote url ).
    public async Task<Candidate> CreateCandidate(int positionid, DateTime dateOfBirth, string emailAddress, string firstname, string surname) {
        var position = await _positionRepository.GetByIdAsync(positionid);

        if (position == null) {
            throw new InvalidOperationException($"Position associated with {positionid} was not found!");
        }

        var credit = await _candidateCreditService.GetCreditAsync(firstname, surname, dateOfBirth);

        switch (position.Name) {
            case "SecuritySpecialist":
                return new SecuritySpecialistCandidate(position, dateOfBirth, emailAddress, firstname, surname, credit, _timeProvider);
            case "FeatureDeveloper":
                return new FeatureDeveloperCandidate(position, dateOfBirth, emailAddress, firstname, surname, credit, _timeProvider);
            default:
                return new Candidate(position, dateOfBirth, emailAddress, firstname, surname, credit, _timeProvider);
        }
    }
}
