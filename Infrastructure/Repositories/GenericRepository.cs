using Core.Context;
using Core.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreDbContext _context;

        public GenericRepository(StoreDbContext context)
        {

            _context = context;
        }

        public async Task Add(T entity)
        => await _context.AddAsync(entity);

        public async Task<int> CountAsync(ISpecification<T> specification)
         => await ApplySpecifications(specification).CountAsync();

        public void Delete(T entity)
         => _context.Set<T>().Remove(entity);
        public async Task<IReadOnlyList<T>> GetAllAsync()
        => await _context.Set<T>().ToListAsync();

        public async Task<IReadOnlyList<T>> GetAllWithSpecificationsAsync(ISpecification<T> specs)
        
            => await ApplySpecifications(specs).ToListAsync();
        

        public async Task<T> GetByIdAsync(int? id)
        => await _context.Set<T>().FindAsync(id);

        public async Task<T> GetEntityWithSpecificationsAsync(ISpecification<T> specs)
            => await ApplySpecifications(specs).FirstOrDefaultAsync();

        public void Update(T entity)
         => _context.Set<T>().Update(entity);

        private IQueryable<T> ApplySpecifications(ISpecification<T> specs)
            =>  SpecificationEvaluater<T>.GetQuery(_context.Set<T>().AsQueryable(), specs);
    }
}
