using Project1.Application.Exceptions;

namespace Project1.Exceptions
{
    public class ApiExceptionsMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiExceptionsMiddleware(RequestDelegate next)
        {
            if (next is null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext is null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            try
            {
                await _next(httpContext);
            }
            catch (ValidationException e)
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteAsync("Validation failed for request: " + e.Message);
                foreach (KeyValuePair<string, string[]> failure in e.Failures)
                {
                    foreach (var message in failure.Value)
                    {
                        await httpContext.Response.WriteAsync(Environment.NewLine + message);
                    }
                }
            }
        }
    }
}
