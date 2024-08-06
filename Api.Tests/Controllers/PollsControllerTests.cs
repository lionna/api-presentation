using Api.Controllers;
using InteractivePresentation.Client.Models;
using InteractivePresentation.Client.Service.Abstract;
using InteractivePresentation.Domain.Model;
using InteractivePresentation.Domain.Service.Abstract;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Api.Tests.Controllers
{
    public class PollsControllerTests
    {
        private readonly Mock<IPresentationClientService> _mockClientService;
        private readonly Mock<IPollService> _mockPollService;
        private readonly PollsController _controller;

        private readonly Guid presentationId = Guid.NewGuid();
        private readonly Guid invalidPresentationId = Guid.Parse("00000000-0000-0000-0000-000000000000");
        private readonly Guid pollId = Guid.NewGuid();
        private string someQuestion = "Sample Question";

        private PollResponse GetPollResponse()
        {
            return new PollResponse { Id = pollId, PresentationId = presentationId, Question = someQuestion };
        }

        public PollsControllerTests()
        {
            _mockClientService = new Mock<IPresentationClientService>();
            _mockPollService = new Mock<IPollService>();
            _controller = new PollsController(_mockClientService.Object, _mockPollService.Object);
        }

        [Fact]
        public async Task GetCurrentPoll_ThrowsArgumentNullException_WhenPresentationIdIsEmpty()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _controller.GetCurrentPoll(invalidPresentationId));
        }

        [Fact]
        public async Task GetCurrentPoll_ReturnsConflict_WhenPollIsNull()
        {
            // Arrange
            const PollResponse? poll = null;
            _mockPollService.Setup(service => service.GetCurrentPollAsync(presentationId))
                            .ReturnsAsync(poll);

            // Act
            var result = await _controller.GetCurrentPoll(presentationId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
            Assert.Equal("There are no polls currently displayed", conflictResult.Value);
        }


        [Fact]
        public async Task GetCurrentPoll_ReturnsOk_WhenPollExists()
        {
            // Arrange
            var poll = GetPollResponse();
            _mockPollService.Setup(service => service.GetCurrentPollAsync(presentationId))
                            .ReturnsAsync(poll);

            // Act
            var result = await _controller.GetCurrentPoll(presentationId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedPoll = Assert.IsType<PollResponse>(okResult.Value);
            Assert.Equal(poll.Id, returnedPoll.Id);
        }

        [Fact]
        public async Task SetCurrentPoll_ThrowsArgumentNullException_WhenPresentationIdIsEmpty()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _controller.SetCurrentPoll(invalidPresentationId));
        }

        [Fact]
        public async Task SetCurrentPoll_ReturnsNotFound_WhenPresentationDoesNotExist()
        {
            // Arrange
            const PresentationResponse? presentationResponse = null;
            _mockClientService.Setup(service => service.GetAsync(presentationId))
                              .ReturnsAsync(presentationResponse);

            // Act
            var result = await _controller.SetCurrentPoll(presentationId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No presentation found.", notFoundResult.Value);
        }

        [Fact]
        public async Task SetCurrentPoll_ReturnsConflict_WhenNoPollsLeft()
        {
            // Arrange
            var presentation = new PresentationResponse { };
            const PollResponseModel? pollResponseModel = null;
            _mockClientService.Setup(service => service.GetAsync(presentationId))
                              .ReturnsAsync(presentation);
            _mockClientService.Setup(service => service.GetCurrentAsync(presentationId))
                              .ReturnsAsync(pollResponseModel);

            // Act
            var result = await _controller.SetCurrentPoll(presentationId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
            Assert.Equal("The presentation ran out of polls.", conflictResult.Value);
        }

        [Fact]
        public async Task SetCurrentPoll_ReturnsOk_WhenPollIsSet()
        {
            // Arrange
            var poll = GetPollResponse();
            var presentation = new PresentationResponse { };
            var clientPoll = new PollResponseModel { PollId = pollId, Question = someQuestion };

            _mockClientService.Setup(service => service.GetAsync(presentationId))
                              .ReturnsAsync(presentation);
            _mockClientService.Setup(service => service.GetCurrentAsync(presentationId))
                              .ReturnsAsync(clientPoll);
            _mockPollService.Setup(service => service.SetCurrentPollAsync(presentationId, It.IsAny<PollRequest>()))
                            .ReturnsAsync(poll);

            // Act
            var result = await _controller.SetCurrentPoll(presentationId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedPoll = Assert.IsType<PollResponse>(okResult.Value);
            Assert.Equal(poll.Id, returnedPoll.Id);
        }
    }
}
