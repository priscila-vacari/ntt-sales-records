using Microsoft.EntityFrameworkCore;
using Sales.Domain.Entities;
using Sales.Infra.Context;
using Sales.Infra.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Sales.Infra.Repositories
{
    [ExcludeFromCodeCoverage]
    public class SaleRepository(AppDbContextPostgre context) : ISaleRepository
    {
        private readonly AppDbContextPostgre _context = context;

        public async Task<IEnumerable<Sale>> GetSalesAsync(int page, int pageSize, string[]? orderBy, string? number, string? customer, string? branch, bool? isCancelled, decimal? totalValueMin, decimal? totalValueMax, params Expression<Func<Sale, object>>[] includes)
        {
            var query = _context.Sales.AsQueryable();

            foreach (var include in includes)
                query = query.Include(include);

            if (!string.IsNullOrEmpty(number))
                query = query.Where(s => s.Branch.Contains(number));

            if (!string.IsNullOrEmpty(customer))
                query = query.Where(s => s.Customer.Contains(customer));

            if (!string.IsNullOrEmpty(branch))
                query = query.Where(s => s.Branch.Contains(branch));

            if (isCancelled != null)
                query = query.Where(s => s.IsCancelled == isCancelled);

            if (totalValueMin.HasValue)
                query = query.Where(s => s.TotalValue >= totalValueMin.Value);

            if (totalValueMax.HasValue)
                query = query.Where(s => s.TotalValue <= totalValueMax.Value);

            if (orderBy != null && orderBy.Length > 0)
            {
                foreach (var order in orderBy)
                {
                    var isDescending = order.StartsWith("-");
                    var propertyName = isDescending ? order[1..] : order;
                    query = ApplyOrdering(query, propertyName, isDescending);
                }
            }

            return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        private static IQueryable<Sale> ApplyOrdering(IQueryable<Sale> query, string propertyName, bool isDescending)
        {
            var parameter = Expression.Parameter(typeof(Sale), "x");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            string methodName = isDescending ? "OrderByDescending" : "OrderBy";
            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(Sale), property.Type);

            return (IQueryable<Sale>)method.Invoke(null, [query, lambda]);
        }
    }
}
