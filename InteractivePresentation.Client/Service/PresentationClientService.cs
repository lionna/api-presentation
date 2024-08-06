using System;
using System.Threading.Tasks;
using InteractivePresentation.Client.Client;
using InteractivePresentation.Client.Client.Abstract;
using InteractivePresentation.Client.Models;
using InteractivePresentation.Client.Service.Abstract;
using Microsoft.Extensions.Options;

namespace InteractivePresentation.Client.Service
{
    public class PresentationClientService(IApiClient apiClient, IOptions<ApplicationSettings> settings)
        : IPresentationClientService
    {
        private readonly ApplicationSettings _apiSettings = settings.Value;

        public async Task<PresentationResponse> GetAsync(Guid presentationId)
        {
            if (presentationId == Guid.Empty)
            {
                throw new ArgumentNullException("PresentationId is null");
            }

            var apiClientRequest = new ApiClientRequest<string>
            {
                Path = $"{_apiSettings.ClientUrl}/presentations/{presentationId}",
            };

            var apiClientResponse = await apiClient.GetAsync<PresentationResponse, string>(apiClientRequest);
            return apiClientResponse?.Data;
        }

        public async Task<PollResponseModel> GetCurrentAsync(Guid presentationId)
        {
            if (presentationId == Guid.Empty)
            {
                throw new ArgumentNullException("PresentationId is null");
            }

            var apiClientRequest = new ApiClientRequest<string>
            {
                Path = $"{_apiSettings.ClientUrl}/presentations/{presentationId}/polls/current",
            };

            var apiClientResponse = await apiClient.GetAsync<PollResponseModel, string>(apiClientRequest);
            return apiClientResponse?.Data;
        }

        public async Task<PresentationResponseModel> PostAsync(PresentationRequest request)
        {
            var apiClientRequest = new ApiClientRequest<PresentationRequest>
            {
                Path = $"{_apiSettings.ClientUrl}/presentations",
                Query = request
            };

            var apiClientResponse = await apiClient.PostAsync<PresentationResponseModel, PresentationRequest>(apiClientRequest);

            return apiClientResponse?.Data;
        }
    }
}
