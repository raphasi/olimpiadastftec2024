namespace CRM.WebApp.Site.Middleware;

public static class RedirectToLoginMiddlewareExtensions
{
    public static IApplicationBuilder UseRedirectToLogin(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RedirectToLoginMiddleware>();
    }
}
