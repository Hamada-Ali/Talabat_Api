using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Specification
{
    public class SpecificationEvaluater<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            var query = inputQuery;

            if(specification.Criteria is not null)
                query = query.Where(specification.Criteria);

            if (specification.OrderBy is not null)
                query = query.OrderBy(specification.OrderBy);

            if (specification.OrderByDecending is not null)
                query = query.OrderByDescending(specification.OrderByDecending);

            if(specification.IsPaginated)
            {
                query = query.Skip(specification.Skip).Take(specification.Take);
            }

            query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}
