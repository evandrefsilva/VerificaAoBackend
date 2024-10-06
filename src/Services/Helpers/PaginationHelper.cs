using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Helpers
{
    public static class PaginationHelper
    {
        public static DataCollection<T> ToPaginator<T>(
            this IQueryable<T> query,
            int page,
            int take)
        {
            var originalPages = page;

            page--;

            if (page > 0)
                page = page * take;

            var result = new DataCollection<T>
            {
                Items = query.Skip(page).Take(take).ToList(),
                Total = query.Count(),
                Page = originalPages
            };

            if (result.Total > 0)
            {
                result.Pages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(result.Total) / take));
            }

            return result;
        }

    }

    public class DataCollection<T>
    {
        public bool HasItems => Items != null && Items.Any();
        public IEnumerable<T> Items { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
        public int Pages { get; set; }
    }

}