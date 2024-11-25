using DynamicQR.Api.Attributes;

namespace DynamicQR.Api.Extensions
{
    internal static class HttpRequestDataExtensions
    {
        /// <summary>
        /// Gets the attribute value from the request header based on the specified attribute type.
        /// </summary>
        /// <typeparam name="T">The type of the attribute.</typeparam>
        /// <param name="req">The HTTP request data.</param>
        /// <returns>The attribute value from the request header.</returns>
        public static string GetAttribute<T>(this Microsoft.Azure.Functions.Worker.Http.HttpRequestData req)
            where T : OpenApiHeaderAttribute
        {
            var headerName = Activator.CreateInstance<T>().Name;

            if (req.Headers.TryGetValues(headerName, out var headerValues))
            {
                return headerValues.First();
            }

            return string.Empty;
        }

        public static bool HasAttribute<T>(this Microsoft.Azure.Functions.Worker.Http.HttpRequestData req)
            where T : OpenApiHeaderAttribute
        {
            var headerName = Activator.CreateInstance<T>().Name;

            return req.Headers.TryGetValues(headerName, out var _);
        }
    }
}
