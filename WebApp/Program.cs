using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Identity;
using MudBlazor.Services;
using Repository;
using Domain;

namespace WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = string.Empty;
#if DEBUGLIVEDATABASE
        connectionString = @"Server=tcp:lifeevents-sql-server.database.windows.net,1433;Initial Catalog=LifeEvents;Persist Security Info=False;User ID=lifeevents;Password=L13fEvEnts!@&&66--;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
#else
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connectionstring 'DefaultConnection' not found.");
#endif
        builder.Services.AddDbContextFactory<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString, p => p.MigrationsAssembly("Domain"));
        }, ServiceLifetime.Scoped);
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddMudServices();
        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<AppDbContext>();
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
        builder.Services.AddScoped<ITenantResolver, TenantResolver>();
        builder.Services.AddAutoMapper(p => p.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));
        builder.Services.AddScoped(typeof(CrossComponentCommunication.CrossComponentCommunication));
        builder.Services.AddScoped<UsersRepository>();
        builder.Services.AddScoped<LifeEventsRepository>();
        builder.Services.AddScoped<MoneyRepository>();
        builder.Services.AddScoped<TagsRepository>();
        builder.Services.AddScoped<Device>();
        builder.Services.AddScoped<DateFilters>();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllers();
        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.Run();
    }
}