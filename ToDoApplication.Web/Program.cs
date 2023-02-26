using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoApplication.Application.DIServiceConfiguration;
using ToDoApplication.Domain.Models;
using ToDoApplication.Infrastructure;
using ToDoApplication.Infrastructure.DIServiceConfiguration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.RegisterServices();
builder.Services.RegisterRepository();
builder.Services.RegisterMappers();
builder.Services.RegisterValidators();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetService<AppDbContext>();

if (dbContext is not null)
{
    var migrations = dbContext.Database.GetPendingMigrations();

    if (migrations.Any())
    {
        dbContext?.Database.Migrate();
    }
}

app.Run();
