using customMiddlewares.Middlewares;

namespace customMiddlewares.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseBadwordHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<JsonBodyMiddleware>()
                      .UseMiddleware<BadWordsHandlerMiddleware>();
        }
    }
}
