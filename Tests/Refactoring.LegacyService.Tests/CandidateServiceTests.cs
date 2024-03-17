using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Refactoring.LegacyService.Tests;

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
        var position = new Position(1, candidateType, "None");
        var dateTime = DateTime.Now.AddYears(-20);

        var mockPositionRepository = new Mock<IPositionRepository>();
        mockPositionRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(position);

        var mockCandidateCreditService = new Mock<ICandidateCreditService>();
        mockCandidateCreditService.Setup(service => service.GetCreditAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).ReturnsAsync(candidateCredit);

        var mockCandidateRepository = new Mock<ICandidateRepository>();

        var fakeNow = new DateTime(2024, 1, 1);
        var mockTimeProvider = new Mock<ITimeProvider>();
        mockTimeProvider.Setup(x => x.Now).Returns(fakeNow);

        var candidateFactory = new CandidateFactory(mockCandidateCreditService.Object, mockPositionRepository.Object, mockTimeProvider.Object);
        var candidateService = new CandidateService(candidateFactory, mockCandidateRepository.Object);

        // Act
        var result = await candidateService.AddCandidate("jason", "xu", "jason.xu@example.com", dateTime, position.Id);

        // Assert
        result.Should().Be(isCandidateAdded);
    }


    [Fact]
    public async Task Adding_Candidate_With_Invalid_Position_Should_Throw_Exception() {
        // Arrange
        var position = new Position(1, "Candidate", "None");
        var dateOfBirth = DateTime.Now.AddYears(-20);

        var mockPositionRepository = new Mock<IPositionRepository>();
        mockPositionRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Position)null);

        var mockCandidateCreditService = new Mock<ICandidateCreditService>();
        mockCandidateCreditService.Setup(service => service.GetCreditAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).ReturnsAsync(700);

        var mockCandidateRepository = new Mock<ICandidateRepository>();

        var fakeNow = new DateTime(2024, 1, 1);
        var mockTimeProvider = new Mock<ITimeProvider>();
        mockTimeProvider.Setup(x => x.Now).Returns(fakeNow);

        var candidateFactory = new CandidateFactory(mockCandidateCreditService.Object, mockPositionRepository.Object, mockTimeProvider.Object);
        var candidateService = new CandidateService(candidateFactory, mockCandidateRepository.Object);

        var action = async () => await candidateService.AddCandidate("jason", "xu", "jason.xu@example.com", dateOfBirth, position.Id);

        // Act and Assert
        await action.Should().ThrowAsync<InvalidOperationException>().WithMessage("Position associated with 1 was not found!");
    }
}

