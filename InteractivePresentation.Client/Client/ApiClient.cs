using System;
using System.Net.Http;
using System.Threading.Tasks;
using InteractivePresentation.Client.Client.Abstract;
using InteractivePresentation.Client.Exceptions;

namespace InteractivePresentation.Client.Client
{
    public class ApiClient(
        HttpClient httpClient) : IApiClient
    {
        private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        public async Task<ApiClientResponse<TResponse>> GetAsync<TResponse, TEntity>(ApiClientRequest<TEntity> apiClientRequest)
            where TResponse : class
            where TEntity : class
        {
            using var httpRequestMessage = apiClientRequest.ToGet();

            return await HandleHttpRequestAsync<TResponse>(httpRequestMessage);
        }

        public async Task<ApiClientResponse<TResponse>> PostAsync<TResponse, TEntity>(ApiClientRequest<TEntity> apiClientRequest) where TResponse : class where TEntity : class
        {
            using var httpRequestMessage = apiClientRequest.ToPostAsForm();

            return await HandleHttpRequestAsync<TResponse>(httpRequestMessage);
        }

        private async Task<ApiClientResponse<TResponse>> HandleHttpRequestAsync<TResponse>(HttpRequestMessage httpRequestMessage)
            where TResponse : class
        {
            ArgumentNullException.ThrowIfNull(httpRequestMessage);

            try
            {
                using var responseMessage = await _httpClient.SendAsync(httpRequestMessage);

                return await responseMessage.ToApiClientResponseAsync<TResponse>();
            }
            catch (Exception exception) when (exception is not UnsuccessfulResponseException)
            {
                throw new UnsuccessfulResponseException("Low-level HTTP request failure", exception);
            }
            catch (Exception exception) when (exception is UnsuccessfulResponseException)
            {
                throw;
            }
        }
    }
}
