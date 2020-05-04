using System.Linq;
using EventsApi.Core.Abstracts;
using EventsApi.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsApi.Infrastracture.Data
{
    public class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T>GetQuery(IQueryable<T> inputQuery,
            ISpecification<T> specification)
        {
            var query = inputQuery;

            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            // Include all includes

            query = specification.Includes
                .Aggregate(query, (current, include) => current.Include(include));


            return query;
        }
    }
}
