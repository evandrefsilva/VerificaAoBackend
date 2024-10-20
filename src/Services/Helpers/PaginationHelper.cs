using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public static class PaginationHelper
    {
        public async static Task<PagedList<T>> ToPagedList<T>(
            this IQueryable<T> query,
            int page,
            int take,
            CancellationToken cancellationToken = default)
        {
            var originalPages = page;

            page--;

            if (page > 0)
                page = page * take;

            var result = new PagedList<T>
            {
                Items = await query.Skip(page).Take(take).ToListAsync(cancellationToken),
                Total = await query.CountAsync(cancellationToken),
                Page = originalPages
            };

            if (result.Total > 0)
            {
                result.Pages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(result.Total) / take));
            }

            return result;
        }

    }

    public class PagedList<T>
    {
        public bool HasItems => Items != null && Items.Any();
        public IEnumerable<T> Items { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
        public int Pages { get; set; }
    }

}