using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventsApi.Core.Abstracts;
using EventsApi.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsApi.Infrastracture.Data
{
    public class EfRepository : IRepository
    {
        private readonly AppDbContext _dbContext;

        public EfRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> AddAsync<T>(T entity) where T : BaseEntity
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await SaveChanges();

            return entity;
        }

        public async Task DeleteAsync<T>(T entity) where T : BaseEntity
        {
             _dbContext.Set<T>().Remove(entity);
            await SaveChanges();
        }

        public Task<T> GetByIdAsync<T>(int id) where T : BaseEntity
        {
            return _dbContext.Set<T>().SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IReadOnlyList<T>> ListAsync<T>() where T : BaseEntity
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync<T>(ISpecification<T> spec) where T : BaseEntity
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync<T>(T entity) where T : BaseEntity
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await SaveChanges();
        }

        private IQueryable<T> ApplySpecification<T>(ISpecification<T> spec) where T : BaseEntity
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
        }
    }
}
