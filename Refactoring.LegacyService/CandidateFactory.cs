using System;
using System.Threading.Tasks;

namespace Refactoring.LegacyService {
    public class CandidateFactory : ICandidateFactory {
        private ICandidateCreditService _candidateCreditService;
        private IPositionRepository _positionRepository;

        // DI for loose couple
        public CandidateFactory(ICandidateCreditService candidateCreditService, IPositionRepository positionRepository) {
            _candidateCreditService = candidateCreditService;
            _positionRepository = positionRepository;
        }

        // For creation of Candidate, we have introduced a factory class for handling what type of Candidate class to create based on positionid.
        // Also async as it's I/O bound task ( as we read credit from remote url ).
        async Task<Candidate> ICandidateFactory.CreateCandidate(int positionid, DateTime dateOfBirth, string emailAddress, string firstname, string surname) {
            var position = await _positionRepository.GetByIdAsync(positionid);
            var credit = await _candidateCreditService.GetCreditAsync(firstname, surname, dateOfBirth);

            switch (position.Name) {
                case "SecuritySpecialist":
                    return new SecuritySpecialistCandidate(position, dateOfBirth, emailAddress, firstname, surname, credit);
                case "FeatureDeveloper":
                    return new FeatureDeveloperCandidate(position, dateOfBirth, emailAddress, firstname, surname, credit);
                default:
                    return new Candidate(position, dateOfBirth, emailAddress, firstname, surname, credit);
            }
        }
    }
}
