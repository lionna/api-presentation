using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InteractivePresentation.Domain.Entity;
using InteractivePresentation.Domain.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace InteractivePresentation.Domain.Repository
{
    public class VoteRepository(ApplicationDbContext context) : IVoteRepository
    {
        public async Task<IEnumerable<Vote>> GetVotesAsync(Guid pollId)
        {
            return await context.Votes.Where(v => v.PollId == pollId).ToListAsync();
        }

        public async Task CreateVoteAsync(Vote vote)
        {
            await context.Votes.AddAsync(vote);
            await context.SaveChangesAsync();
        }
    }
}