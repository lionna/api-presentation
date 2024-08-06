using InteractivePresentation.Client.Client;
using InteractivePresentation.Client.Client.Abstract;
using InteractivePresentation.Client.Models;
using InteractivePresentation.Client.Service;
using InteractivePresentation.Client.Service.Abstract;
using Microsoft.Extensions.Options;
using Moq;

namespace InteractivePresentation.Client.Tests.Service
{
    public class PresentationClientServiceTests
    {
        private readonly Mock<IApiClient> _mockApiClient;
        private readonly Mock<IOptions<ApplicationSettings>> _mockSettings;
        private readonly ApplicationSettings _appSettings;
        private readonly IPresentationClientService _service;

        private readonly Guid invalidPresentationId = Guid.Parse("00000000-0000-0000-0000-000000000000");
        private readonly Guid presentationId = Guid.NewGuid();
        private const string someQuestion = "Test Question";

        public PresentationClientServiceTests()
        {
            _mockApiClient = new Mock<IApiClient>();
            _appSettings = new ApplicationSettings { ClientUrl = "http://test-url" };
            _mockSettings = new Mock<IOptions<ApplicationSettings>>();
            _mockSettings.Setup(s => s.Value).Returns(_appSettings);

            _service = new PresentationClientService(_mockApiClient.Object, _mockSettings.Object);
        }

        [Fact]
        public async Task GetAsync_ValidPresentationId_ReturnsPresentationResponse()
        {
            // Arrange
            var expectedResponse = new ApiClientResponse<PresentationResponse>
            {
                Data = new PresentationResponse {  }
            };

            _mockApiClient.Setup(c => c.GetAsync<PresentationResponse, string>(It.IsAny<ApiClientRequest<string>>()))
                          .ReturnsAsync(expectedResponse);

            // Act
            var result = await _service.GetAsync(presentationId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAsync_EmptyPresentationId_ThrowsArgumentNullException()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _service.GetAsync(invalidPresentationId));
            Assert.StartsWith("Value cannot be null", exception.Message);
        }

        [Fact]
        public async Task GetCurrentAsync_ValidPresentationId_ReturnsPollResponseModel()
        {
            // Arrange
            var expectedResponse = new ApiClientResponse<PollResponseModel>
            {
                Data = new PollResponseModel { PollId = Guid.NewGuid(), Question = someQuestion }
            };

            _mockApiClient.Setup(c => c.GetAsync<PollResponseModel, string>(It.IsAny<ApiClientRequest<string>>()))
                          .ReturnsAsync(expectedResponse);

            // Act
            var result = await _service.GetCurrentAsync(presentationId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(someQuestion, result.Question);
        }

        [Fact]
        public async Task GetCurrentAsync_EmptyPresentationId_ThrowsArgumentNullException()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _service.GetCurrentAsync(invalidPresentationId));
            Assert.StartsWith("Value cannot be null", exception.Message);
        }

        [Fact]
        public async Task PostAsync_ValidRequest_ReturnsPresentationResponseModel()
        {
            // Arrange
            var request = new PresentationRequest {   };
            var expectedResponse = new ApiClientResponse<PresentationResponseModel>
            {
                Data = new PresentationResponseModel { PresentationId = presentationId }
            };

            _mockApiClient.Setup(c => c.PostAsync<PresentationResponseModel, PresentationRequest>(It.IsAny<ApiClientRequest<PresentationRequest>>()))
                          .ReturnsAsync(expectedResponse);

            // Act
            var result = await _service.PostAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(presentationId, result.PresentationId);
        }
    }
}