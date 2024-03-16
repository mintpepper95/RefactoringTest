using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Refactoring.LegacyService.Tests {
    public class CandidateServiceTests {
        [Theory]
        [InlineData("Candidate", 1000, true)]
        [InlineData("SecuritySpecialist", 1000, true)]
        [InlineData("FeatureDeveloper", 500, true)]
        [InlineData("Candidate", 100, true)]
        [InlineData("SecuritySpecialist", 900, false)]
        [InlineData("FeatureDeveloper", 400, false)]
        public async Task Adding_Candidate_Should_Return_Expected__Result(string candidateType, int candidateCredit, bool isCandidateAdded) {
            // Arrange
            var mockConfigurationManagerWrapper = new Mock<IConfigurationManagerWrapper>();
            mockConfigurationManagerWrapper.Setup(wrapper => wrapper.GetConnectionString()).Returns("test.connection.string");

            var mockPositionRepository = new Mock<PositionRepository>(mockConfigurationManagerWrapper.Object);
            var position = new Position(1, candidateType, "None");
            mockPositionRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(position);

            var mockCandidateCreditService = new Mock<ICandidateCreditService>();
            mockCandidateCreditService.Setup(service => service.GetCreditAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).ReturnsAsync(candidateCredit);

            var mockCandidateRepository = new Mock<ICandidateRepository>();

            var candidateFactory = new CandidateFactory(mockCandidateCreditService.Object, mockPositionRepository.Object);
            var candidateService = new CandidateService(candidateFactory, mockCandidateRepository.Object);
            var positionid = 1;
            var dateTime = DateTime.Now.AddYears(-20);

            // Act
            var result = await candidateService.AddCandidate("jason", "xu", "jason.xu@example.com", dateTime, positionid);

            // Assert
            result.Should().Be(isCandidateAdded);
        }
    }
}
