using System;

namespace Refactoring.LegacyService;

public class Candidate {
    public Position Position { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public string EmailAddress { get; private set; }
    public string Firstname { get; private set; }
    public string Surname { get; private set; }
    public virtual bool RequireCreditCheck { get; private set; } = false;
    public virtual int Credit { get; private set; }

    // The idea is that we don't bother creating the Candidate given invalid parameter values.
    // Such as invalid names and emails or underage, as it does not make sense.
    // Note this is different to the logic of determining whether we should actually add a Candidate to the database,
    // eg. using their credits. That logic should be handled by the appropriate service that adds the Candidate to the database.
    public Candidate(Position position, DateTime dateOfBirth, string emailAddress, string firstname, string surname, int credit, ITimeProvider timeProvider) {
        if (string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(surname)) {
            throw new ArgumentException("First name and last name can't be null or blank");
        }
        // @@@@....   <= this would pass as a valid email, but readme says to assume the logic is perfectly sound so I won't temper with it.
        if (!emailAddress.Contains("@") || !emailAddress.Contains(".")) {
            throw new ArgumentException("Email entered is not a valid email", "emailAddress");
        }
        var age = CalculateAge(dateOfBirth, timeProvider);
        if (age < 18) {
            throw new ArgumentException("Candidate age must be 18 or over", "dateOfBirth");
        }

        Position = position;
        DateOfBirth = dateOfBirth;
        EmailAddress = emailAddress;
        Firstname = firstname;
        Surname = surname;
        Credit = credit;
    }

    // CalculateAge() has been moved from CandidateService to Candidate.
    // Because it doesn't make sense to create a Candidate who's under 18,
    // just like it doesn't make sense to create a Candidate with invalid name or email.
    private int CalculateAge(DateTime dateOfBirth, ITimeProvider timeProvider) {
        var now = timeProvider.Now;
        int age = now.Year - dateOfBirth.Year;

        if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) {
            age -= 1;
        }
        return age;
    }
}


// Note Candidates may have their credits and requireCreditChecks adjusted depending on the particular role they've taken.
// To follow the open closed principle, we introduce derived classes SecuritySpecialist and FeatureDeveloper.
// This ensures each role can have its own RequiredCreditCheck and Credit, and we don't need to adjust these values
// anywhere else ( for example in CandidateService ) anytime we introduce a new candidate role which reduces coupling.
public class SecuritySpecialistCandidate : Candidate {
    public SecuritySpecialistCandidate(Position position, DateTime dateOfBirth, string emailAddress, string firstname, string surname, int credit, ITimeProvider timeProvider)
        : base(position, dateOfBirth, emailAddress, firstname, surname, credit, timeProvider) { }

    public override bool RequireCreditCheck => true;

    public override int Credit => base.Credit / 2;
}

public class FeatureDeveloperCandidate : Candidate {
    public FeatureDeveloperCandidate(Position position, DateTime dateOfBirth, string emailAddress, string firstname, string surname, int credit, ITimeProvider timeProvider)
        : base(position, dateOfBirth, emailAddress, firstname, surname, credit, timeProvider) { }

    public override bool RequireCreditCheck => true;
}
