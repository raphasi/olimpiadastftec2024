using Microsoft.AspNetCore.Authorization;

namespace CRM.WebApp.Ingresso.Middleware
{
    public class RedirectToLoginMiddleware
    {
        private readonly RequestDelegate _next;

        public RedirectToLoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var accessToken = context.Session.GetString("access_token") ?? context.Request.Cookies["access_token"];

            // Verifica se a rota exige autenticação
            var endpoint = context.GetEndpoint();
            var authorizeAttribute = endpoint?.Metadata.GetMetadata<AuthorizeAttribute>();

            if (authorizeAttribute != null && string.IsNullOrEmpty(accessToken))
            {
                context.Response.Redirect("/Account/Login");
                return;
            }

            await _next(context);
        }

    }
}
