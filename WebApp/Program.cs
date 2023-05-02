using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Identity;
using WebApp.Data;
using MudBlazor.Services;
using Repository;
using CrossComponentCommunication;
using Domain;

namespace WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContextFactory<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString, p => p.MigrationsAssembly("Domain"));
        },ServiceLifetime.Scoped);
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
        builder.Services.AddScoped<TagsRepository>();
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
