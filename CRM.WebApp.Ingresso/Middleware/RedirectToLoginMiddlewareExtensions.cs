namespace CRM.WebApp.Ingresso.Middleware;

public static class RedirectToLoginMiddlewareExtensions
{
    public static IApplicationBuilder UseRedirectToLogin(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RedirectToLoginMiddleware>();
    }
}
