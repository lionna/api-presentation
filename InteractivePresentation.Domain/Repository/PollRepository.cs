using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InteractivePresentation.Domain.Entity;
using InteractivePresentation.Domain.Exceptions;
using InteractivePresentation.Domain.Repository.Abstract;

namespace InteractivePresentation.Domain.Repository
{
    public class PollRepository(
        IGenericRepositoryAsync<Poll, Guid> repository,
        ApplicationDbContext context) : IPollRepository
    {
        public async Task<IEnumerable<Poll>> GetPollsAsync(Guid presentationId)
        {
            return (await repository.GetAllAsync()).Where(p => p.PresentationId == presentationId).ToList();
        }

        public async Task<Poll> GetCurrentPollAsync(Guid presentationId)
        {
            return (await repository.GetAllAsync()).FirstOrDefault(p => p.PresentationId == presentationId && p.IsCurrent);
        }

        public async Task CreatePollAsync(Poll poll)
        {
            await repository.CreateAsync(poll);
        }

        public async Task SetCurrentPollAsync(Guid presentationId, Poll poll)
        {
            //using var cancellationTokenSource = new CancellationTokenSource();
            //var cancellationToken = cancellationTokenSource.Token;
            using (var transaction = await context.Database.BeginTransactionAsync()) //IsolationLevel.Serializable
            {
                try
                {
                    var entitiesToUpdate = (await repository.GetAllAsync()).Where(p => p.PresentationId == presentationId).ToList();
                    foreach (var entity in entitiesToUpdate)
                    {
                        entity.IsCurrent = false;
                    }
                    await repository.SaveChangesAsync();
                    await repository.CreateAsync(poll);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new BeginTransactionException("Transaction rollback", ex);
                }
            }
        }

        public async Task UpdatePollAsync(Poll poll)
        {
            await repository.UpdateAsync(poll);
        }

        public async Task<Poll> GetPollByIdAsync(Guid pollId)
        {
           return await repository.GetByIdAsync(pollId);
        }
    }
}