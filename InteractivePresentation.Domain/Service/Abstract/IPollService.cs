using InteractivePresentation.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteractivePresentation.Domain.Service.Abstract
{
    public interface IPollService
    {
        Task<PollResponse> GetCurrentPollAsync(Guid presentationId);
        Task<PollResponse> SetCurrentPollAsync(Guid presentationId, PollRequest poll);
        Task CreateVoteAsync(Guid presentationId, Guid pollId, VoteRequest vote);
        Task<IEnumerable<VoterResponse>> GetVotesAsync(Guid presentationId, Guid pollId);
    }
}
