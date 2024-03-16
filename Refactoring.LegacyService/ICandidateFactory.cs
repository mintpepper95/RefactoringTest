using System;
using System.Threading.Tasks;

namespace Refactoring.LegacyService;

public interface ICandidateFactory {
    Task<Candidate> CreateCandidate(int position, DateTime dateOfBirth, string emailAddress, string firstname, string surname);
}
