using AutoMapper;
using hospins.Extensions;
using hospins.Infrastructure;
using hospins.Repository.Data;
using hospins.Repository.ServiceContract;
using hospins.Repository.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddSession(options =>
{
    // Set a short timeout for easy testing.
    options.IdleTimeout = TimeSpan.FromHours(4);
    options.Cookie.Name = ".hospins.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.Path = "/";
});
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN";

    options.Cookie.Name = "XSRF-TOKEN";
    options.Cookie.HttpOnly = true;
    //options.Cookie.Domain = Request.RequestUri.Host;
    options.Cookie.Path = "/";
    //options.Cookie.SameSite = SameSiteMode.Strict;
    //options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = _ => false;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

builder.Services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));

builder.Services.AddEntityFrameworkSqlServer();
builder.Services.AddDbContextPool<hospinsContext>((provider, options) =>
{
    options.UseSqlServer(configuration.GetConnectionString("strDBConn"));
    options.UseInternalServiceProvider(provider);
});

#region Services
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient(typeof(ICommonRepository<>), typeof(CommonRepository<>));
builder.Services.AddTransient<ICheckExistingRepository, CheckExistingRepository>();
#endregion

//Auto Mapper Configurations
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var mapperConfiguration = new MapperConfiguration(configure => configure.AddProfile<ApplicationMappingProfile>());
mapperConfiguration.CreateMapper().InitializeMapper();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        //ctx.Context.Response.Headers.Append("Cache-Control", "public, max-age=7200"); //2hr
        ctx.Context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    }
});

app.UseRouting();

app.UseCookiePolicy();
app.UseRouting();
app.UseAuthorization();
app.UseSession();

app.MapRazorPages();

var connectionString = configuration.GetConnectionString("strDBConn");
hospinsContext.SetConnectionString(connectionString);

app.MapControllerRoute(
    name: "EmployeePanel",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
