using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InteractivePresentation.Domain.Entity.Abstract;

namespace InteractivePresentation.Domain.Repository.Abstract
{
    public interface IGenericRepositoryAsync<T, TKey>
        where T : class, ICommonEntity<TKey>
        where TKey : IComparable<TKey>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(TKey id);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(TKey id);
        Task SaveChangesAsync();
    }
}
