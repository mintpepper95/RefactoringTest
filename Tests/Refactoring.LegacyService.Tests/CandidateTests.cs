using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace Refactoring.LegacyService.Tests {
    public class CandidateTests {
        [Theory]
        [InlineData("", "", "First name and last name can't be null or blank")]
        [InlineData("", "xu", "First name and last name can't be null or blank")]
        [InlineData("jason", "", "First name and last name can't be null or blank")]
        [InlineData(null, null, "First name and last name can't be null or blank")]
        [InlineData("jason", null, "First name and last name can't be null or blank")]
        [InlineData(null, "xu", "First name and last name can't be null or blank")]
        public void Adding_Candidate_Should_Throw_Exception_When_First_Or_Last_Name_Is_Empty_Or_Null(string firstName, string lastName, string exMessage) {
            // Arrange
            var positionid = 1;
            var position = new Position(positionid, "SecuritySpecialist", "None");
            var dateTime = DateTime.Now.AddYears(-20);

            // Act
            Action action = () => new Candidate(position, dateTime, "jason.xu@example.com", firstName, lastName, 700);

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage(exMessage);
        }

        [Theory]
        [InlineData("Candidate", 800, 800, false)]
        [InlineData("SecuritySpecialist", 1400, 700, true)]
        [InlineData("FeatureDeveloper", 700, 700, true)]
        public void Adding_Candidate_Should_Return_A_Candidate_With_Correct_Credit_And_BackgroundCheckRequirement(string candidateType, int candidateCredit, int expectedCredit, bool isCreditCheckRequired) {
            // Arrange
            var position = new Position(1, "SecuritySpecialist", "None");
            var dateTime = DateTime.Now.AddYears(-20);

            // Act
            Candidate candidate;
            switch (candidateType) {
                case "SecuritySpecialist":
                    candidate = new SecuritySpecialistCandidate(position, dateTime, "jason@example.com", "jason", "xu", candidateCredit);
                    break;
                case "FeatureDeveloper":
                    candidate = new FeatureDeveloperCandidate(position, dateTime, "jason@example.com", "jason", "xu", candidateCredit);
                    break;
                default:
                    candidate = new Candidate(position, dateTime, "jason@example.com", "jason", "xu", candidateCredit);
                    break;
            }

            // Assert
            candidate.Credit.Should().Be(expectedCredit);
            candidate.RequireCreditCheck.Should().Be(isCreditCheckRequired);
        }

        [Theory]
        [InlineData("test.hotmail.com", "Email entered is not a valid email (Parameter 'emailAddress')")]
        [InlineData("", "Email entered is not a valid email (Parameter 'emailAddress')")]
        public void Adding_Candidate_Should_Throw_Exception_With_Incorrect_Email_Format(string email, string exMessage) {
            // Arrange
            var mockCandidateFactory = new Mock<ICandidateFactory>();
            var mockCandidateRepository = new Mock<ICandidateRepository>();
            var position = new Position(1, "SecuritySpecialist", "None");
            var dateTime = DateTime.Now.AddYears(-20);
            Action action = () => new Candidate(position, dateTime, email, "jason", "xu", 700);

            // Act and Assert
            action.Should().Throw<ArgumentException>().WithMessage(exMessage);
        }

        [Fact]
        public void Adding_Candidate_Should_Throw_Exception_With_Age_Under_18() {
            // Arrange
            var mockCandidateFactory = new Mock<ICandidateFactory>();
            var mockCandidateRepository = new Mock<ICandidateRepository>();
            var position = new Position(1, "SecuritySpecialist", "None");
            var dateTime = DateTime.Now.AddYears(-17);
            Action action = () => new Candidate(position, dateTime, "jason@example.com", "jason", "xu", 700);

            // Act and Assert
            action.Should().Throw<ArgumentException>().WithMessage("Candidate age must be 18 or over (Parameter 'dateOfBirth')");
        }
    }
}
