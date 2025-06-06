using Chat.Application.Models.Pagination;
using Chat.Common;
using Chat.Contracts.ApiContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

namespace Chat.Application.Extensions
{
    internal static class PaginationExtension
    {
        private static readonly ILogger _logger = LoggerUtil.CreateLogger(nameof(PaginationExtension));

        public async static Task<Paginated<TData>> AsPaginatedAsync<TData>(this IQueryable<TData> data, PaginationParams paginationParams, CancellationToken cancellationToken, SortParams? sortParams = default)
            => await data.AsPaginatedAsync(paginationParams, static s => s, cancellationToken, sortParams);

        public async static Task<Paginated<TData>> AsPaginatedAsync<TData, TSource>(this IQueryable<TSource> data, PaginationParams paginationParams, Func<TSource, TData> map, CancellationToken cancellationToken, SortParams? sortParams = default)
        {
            try
            {
                data = data.ApplySorting(sortParams);

                var result = await data
                    .Skip(paginationParams.PageNumber * paginationParams.PageSize)
                    .Take(paginationParams.PageSize)
                    .ToListAsync(cancellationToken);

                var totalItems = data.LongCount();
                var totalPages = (int)Math.Ceiling((double)(totalItems / paginationParams.PageSize));

                return new Paginated<TData>()
                {
                    Items = [.. result.Select(s => map(s))],
                    TotalPages = totalPages + 1,
                    HasPreveiwPage = paginationParams.PageNumber > PaginationParams.StartPage,
                    HasNextPage = paginationParams.PageNumber < totalPages
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Pagination failed due to the following error: {error}", ex.Message);
                throw;
            }
        }

        private static IQueryable<TSource> ApplySorting<TSource>(this IQueryable<TSource> data, SortParams? sortParams)
        {
            if (sortParams is not null)
            {
                var expression = $"{sortParams.SortBy} {(sortParams.IsAscending ? "asc" : "desc")}";
                return data.OrderBy(expression);
            }
            else
            {
                return data.Order();
            }
        }
    }
}
