using System.Threading.Tasks;

namespace InteractivePresentation.Client.Client.Abstract
{
    public interface IApiClient
    {
        Task<ApiClientResponse<TResponse>> GetAsync<TResponse, TEntity>(ApiClientRequest<TEntity> apiClientRequest)
            where TResponse : class
            where TEntity : class;

        Task<ApiClientResponse<TResponse>> PostAsync<TResponse, TEntity>(ApiClientRequest<TEntity> apiClientRequest)
            where TResponse : class
            where TEntity : class;
    }
}
