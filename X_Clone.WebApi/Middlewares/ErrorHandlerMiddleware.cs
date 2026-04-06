using XClone.Domain.Exceptions;

namespace XClone.WebApi.Middlewares
{
    public class ErrorHandlerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);


            }
            catch (NotFoundException exception)
            {
                throw;
            }
        }
    }
}
