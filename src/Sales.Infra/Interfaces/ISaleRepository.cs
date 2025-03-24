using Sales.Domain.Entities;
using System.Linq.Expressions;

namespace Sales.Infra.Interfaces
{
    public interface ISaleRepository
    {
        Task<IEnumerable<Sale>> GetSalesAsync(int page, int pageSize, string[]? orderBy, string? number, string? customer, string? branch, bool? isCancelled, decimal? totalValueMin, decimal? totalValueMax, params Expression<Func<Sale, object>>[] includes);
    }
}
