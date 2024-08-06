using InteractivePresentation.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteractivePresentation.Domain.Repository.Abstract
{
    public interface IPollRepository
    {
        Task<IEnumerable<Poll>> GetPollsAsync(Guid presentationId);
        Task<Poll> GetCurrentPollAsync(Guid presentationId);
        Task CreatePollAsync(Poll poll);
        Task SetCurrentPollAsync(Guid presentationId, Poll poll);
        Task UpdatePollAsync(Poll poll);
        Task<Poll> GetPollByIdAsync(Guid pollId);
    }
}
