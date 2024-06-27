using CRM.CrossCutting.IoC;
using CRM.WebApp.Ingresso.Models;

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

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Store}/{action=Index}/{id?}");

app.Run();


