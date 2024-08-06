using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InteractivePresentation.Domain.Entity;

namespace InteractivePresentation.Domain.Repository.Abstract
{
    public interface IVoteRepository
    {
        Task<IEnumerable<Vote>> GetVotesAsync(Guid pollId);
        Task CreateVoteAsync(Vote vote);
    }
}
