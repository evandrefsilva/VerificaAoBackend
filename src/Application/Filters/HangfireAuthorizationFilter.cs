using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace Application.Filters
{
    using Hangfire.Dashboard;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
           
            // Allow only users iiu jhkkn the Admin role to see the Dashboard.
            //var isInRole = httpContext.User.IsInRole("admin");
            var isInRole = true;
            return  isInRole;
        }
    }

}
