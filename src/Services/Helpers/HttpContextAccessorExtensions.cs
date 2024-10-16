using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Services.Helpers
{
    public static class HttpContextAccessorExtensions
    {
        public static string GetBaseUrl(this IHttpContextAccessor httpContextAccessor)
        {
            var request = httpContextAccessor.HttpContext?.Request;

            if (request == null)
            {
                return null; // ou trate como necessário
            }

            // Constrói a URL base (por exemplo, https://localhost:5001)
            return $"{request.Scheme}://{request.Host}";
        }
    }

}
