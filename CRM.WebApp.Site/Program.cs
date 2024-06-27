using CRM.CrossCutting.IoC;
using CRM.Infrastructure.Context;
using CRM.WebApp.Site.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Configuração do HttpClient
builder.Services.AddHttpClient("CRM.API", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]);

});

builder.Services.AddDistributedMemoryCache(); // Necessário para armazenar sessões na memória
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(1); // Tempo de expiração da sessão
    options.Cookie.HttpOnly = true; // Torna o cookie de sessão acessível apenas via HTTP
    options.Cookie.IsEssential = true; // Necessário para conformidade com GDPR
});

builder.Services.Configure<ConfigurationImageViewModel>(options =>
{
    options.NomePastaImagensProdutos = builder.Configuration["ConfigurationPastaImagens:NomePastaImagensProdutos"];
});

builder.Services.AddInfrastructureJWT(builder.Configuration);

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configuração do pipeline de requisições HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseStatusCodePages(async context =>
{
    if (context.HttpContext.Response.StatusCode == 401)
    {
        context.HttpContext.Response.Redirect("/Account/AccessDenied");
    }
    else if (context.HttpContext.Response.StatusCode == 403)
    {
        context.HttpContext.Response.Redirect("/Account/AccessDenied");
    }
});

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();