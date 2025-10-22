using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TorneoSolar.Models;
using TorneoSolar.Servicios.Contrato;
using TorneoSolar.Servicios.Implementacion;
using TorneoSolar.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<TorneoSolarContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));
builder.Services.AddScoped<IUsuarioServices, UsuariosSevices>();
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.LoginPath = "/Inicio/IniciarSesion";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax;
});
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(
    new ResponseCacheAttribute
    {
        NoStore = true,
        Location = ResponseCacheLocation.None,
    }
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Apply pending EF Core migrations at startup (dev-friendly)
try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<TorneoSolarContext>();
    db.Database.Migrate();
}
catch (Exception ex)
{
    // In Development, it's helpful to see migration errors early
    Console.WriteLine($"[Startup] Error applying migrations: {ex.Message}");
    // Rethrow to avoid running with a broken schema
    throw;
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Admin",
    pattern: "Admin",
    defaults: new { controller = "Home", action = "Admin" });

app.Run();
