using System;
using System.Threading.Tasks;
using InteractivePresentation.Client.Models;

namespace InteractivePresentation.Client.Service.Abstract
{
    public interface IPresentationClientService
    {
        Task<PresentationResponse> GetAsync(Guid presentationId);
        Task<PollResponseModel> GetCurrentAsync(Guid presentationId);
        Task<PresentationResponseModel> PostAsync(PresentationRequest request);
    }
}
