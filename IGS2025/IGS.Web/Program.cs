using Globalsetting;
using IGS.Dal.Data;
using IGS.Dal.Repository;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Repository.Repository;
using IGS.Dal.Services;
using IGS.Dal.Sql;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// 🔗 Connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 🔐 Identity cookie config → ensure redirect goes to /igsadmin
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = new PathString("/igsadmin");       // ✅ force login path
    options.LogoutPath = new PathString("/Identity/Account/Logout");
    options.AccessDeniedPath = new PathString("/igsadmin"); // redirect access denied to login
});

// 🛠 Dev helpers
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// 🔐 Identity setup
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IEmailSender, NoOpEmailSender>();

// 👇 Map /igsadmin → Identity login page
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "/igsadmin");
});

// 🧰 App services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<GlobalEnvironmentSetting>();
builder.Services.AddScoped<GlobalCookies>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 🗂 Repositories & services
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddScoped<ISqlHelper, SqlHelper>();
builder.Services.AddScoped<ILoggerService, LoggerService>();
builder.Services.AddScoped<ICommonListingService, CommonListingService>();

var app = builder.Build();

// 🔥 Global error handler → logs exceptions to DB
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        using var scope = app.Services.CreateScope();
        var loggerService = scope.ServiceProvider.GetRequiredService<ILoggerService>();
        await loggerService.LogErrorAsync(ex);
        throw; // rethrow so exception page / handler still works
    }
});

// 🌍 Pipeline
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

// Static files with cache headers
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=2592000");
    }
});

app.UseRouting();

// 🚦 Lightweight redirect from /Account/Login → /igsadmin
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/Account/Login", StringComparison.OrdinalIgnoreCase))
    {
        context.Response.Redirect("/igsadmin" + context.Request.QueryString);
        return;
    }
    await next();
});

// ✅ correct order
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
