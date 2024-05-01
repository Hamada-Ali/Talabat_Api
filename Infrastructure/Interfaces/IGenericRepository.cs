using Core.Entities;
using Infrastructure.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int? id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetEntityWithSpecificationsAsync(ISpecification<T> specs);
        Task<IReadOnlyList<T>> GetAllWithSpecificationsAsync(ISpecification<T> specs);
        Task<int> CountAsync(ISpecification<T> specification);
        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
