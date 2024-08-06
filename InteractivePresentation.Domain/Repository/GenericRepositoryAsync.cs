using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InteractivePresentation.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using InteractivePresentation.Domain.Entity.Abstract;
using InteractivePresentation.Domain.Repository.Abstract;

namespace InteractivePresentation.Domain.Repository
{
    public class GenericRepositoryAsync<T, TKey>(ApplicationDbContext context)
        : IGenericRepositoryAsync<T, TKey>
        where T : class, ICommonEntity<TKey>
        where TKey : IComparable<TKey>
    {
        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(TKey id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task CreateAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TKey id)
        {
            var entityToDelete = await _context.Set<T>().FindAsync(id)
                ?? throw new InvalidOperationException($"Entity with id {id} not found.");
            _context.Set<T>().Remove(entityToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

