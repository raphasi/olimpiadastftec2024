using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace CRM.CrossCutting.IoC;

public static class DependencyInjectionEntraId
{
    public static IServiceCollection AddInfrastructureEntraId(this IServiceCollection services, IConfiguration configuration)
    {
        var secretKey = configuration["Jwt:Key"] ?? throw new ArgumentException("Chave de acesso inválida");

        // Adiciona serviços de localização
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[] { new CultureInfo("pt-BR") };
            options.DefaultRequestCulture = new RequestCulture("pt-BR");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });


        // Configuração de Autorização
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
            // Adicione outras políticas conforme necessário
        });

        // Configura o esquema de autenticação JWT-Bearer
        services.AddAuthentication(opt =>
        {
            opt.DefaultScheme = AzureADDefaults.AuthenticationScheme;
        }).AddCookie(options =>
        {
            options.LoginPath = "/Account/Login"; // Caminho para a página de login
            options.AccessDeniedPath = "/Account/AccessDenied"; // Caminho para a página de acesso negado
        })            // Configura a validação do token JWT
            .AddJwtBearer("AzureAD", options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // Ler o token do cookie
                        var token = context.Request.Cookies["access_token"];
                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    }
                };
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    // Valores válidos para a validação
                    ValidIssuer = configuration["AzureAD:Issuer"],
                    ValidAudience = configuration["AzureAD:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ClockSkew = TimeSpan.Zero // Elimina o tempo de tolerância padrão para expiração do token
                };
            });

        // Adiciona o serviço CORS com uma política permissiva
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });

        return services;
    }
}