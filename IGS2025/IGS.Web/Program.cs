using Globalsetting;
using IGS.Dal.Data;
using IGS.Dal.Repository;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Repository.Repository;
using IGS.Dal.Services;
using IGS.Dal.Services.Implementations;
using IGS.Dal.Services.Interfaces;
using IGS.Dal.Sql;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation; // ✅ Required for AddRazorRuntimeCompilation
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

//
// 🔗 Connection string
//
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

//
// ✅ EF Core registration
//
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions => sqlOptions.CommandTimeout(180)));

//
// 🔐 Identity setup
//
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = new PathString("/igsadmin");
    options.LogoutPath = new PathString("/Identity/Account/Logout");
    options.AccessDeniedPath = new PathString("/igsadmin");
});

//
// 🧩 Gzip + Brotli compression setup
//
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
    {
        "text/plain",
        "text/html",
        "text/css",
        "application/javascript",
        "application/json",
        "application/xml",
        "text/xml",
        "text/json",
        "image/svg+xml"
    });
});

builder.Services.Configure<BrotliCompressionProviderOptions>(o => o.Level = CompressionLevel.Fastest);
builder.Services.Configure<GzipCompressionProviderOptions>(o => o.Level = CompressionLevel.Fastest);

//
// 🛠 Developer helpers
//
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

#if DEBUG
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation(); // ✅ Only in Debug mode
#else
builder.Services.AddControllersWithViews();
#endif

builder.Services.AddSingleton<IEmailSender, NoOpEmailSender>();

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "/igsadmin");
});

//
// 🧰 Application-wide services
//
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

//
// 🗂 Repository + Unit of Work
//
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddScoped<ISqlHelper, SqlHelper>();
builder.Services.AddScoped<ILoggerService, LoggerService>();
builder.Services.AddScoped<ICommonListingService, CommonListingService>();

//
// 💼 Business Services
//
builder.Services.AddScoped<IHomeVmService, HomeVmService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<ITransactionServicesVmService, TransactionServicesVmService>();
builder.Services.AddScoped<ITransactionServicesService, TransactionServicesService>();
builder.Services.AddScoped<IPortfolioServicesVmService, PortfolioServicesVmService>();
builder.Services.AddScoped<IPortfolioServicesService, PortfolioServicesService>();
builder.Services.AddScoped<IIndustryCategoryService, IndustryCategoryService>();
builder.Services.AddScoped<IIndustryService, IndustryService>();
builder.Services.AddScoped<IIndustryVmService, IndustryVmService>();
builder.Services.AddScoped<IExperienceVmService, ExperienceVmService>();
builder.Services.AddScoped<IExperienceService, ExperienceService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ITeamVmService, TeamVmService>();
builder.Services.AddScoped<ITeamTitleService, TeamTitleService>();
builder.Services.AddScoped<ITeamTitleVmService, TeamTitleVmService>();
builder.Services.AddScoped<INewsVmService, NewsVmService>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<IContactVmService, ContactVmService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IPrivacyPolicyService, PrivacyPolicyService>();
builder.Services.AddScoped<IPrivacyPolicyVmService, PrivacyPolicyVmService>();
builder.Services.AddScoped<ICommonService, CommonService>();
builder.Services.AddScoped<INewsLetterVmService, NewsLetterVmService>();

var app = builder.Build();

//
// 🌐 Global exception logging middleware
//
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
        throw;
    }
});

//
// 🧱 Error handling
//
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithReExecute("/Home/HandleError/{0}");
    app.UseHsts();
}

//
// ⚙️ Middleware pipeline
//
app.UseHttpsRedirection();
app.UseResponseCompression();

//
// ✅ Static file caching setup
//
var webRoot = app.Environment.WebRootPath;

void UseCachedStaticFolder(string folderName, string requestPath, int durationDays)
{
    var folderPath = Path.Combine(webRoot, folderName);
    if (Directory.Exists(folderPath))
    {
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(folderPath),
            RequestPath = requestPath,
            OnPrepareResponse = ctx =>
            {
                int duration = 60 * 60 * 24 * durationDays;
                ctx.Context.Response.Headers["Cache-Control"] = "public,max-age=" + duration;
                ctx.Context.Response.Headers["Expires"] = DateTime.UtcNow.AddSeconds(duration).ToString("R");
            }
        });
    }
}

// 🖼️ Images → 1 year
UseCachedStaticFolder("images", "/images", 365);

// 🧩 JS → 30 days
UseCachedStaticFolder("js", "/js", 30);

// 🎨 CSS → 30 days
UseCachedStaticFolder("css", "/css", 30);

// 🔤 Fonts → 1 year
UseCachedStaticFolder("fonts", "/fonts", 365);

// 📄 PDFs → 1 year
UseCachedStaticFolder("pdf", "/pdf", 365);

// 📦 Fallback default static files → 7 days
if (Directory.Exists(webRoot))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        OnPrepareResponse = ctx =>
        {
            const int duration = 60 * 60 * 24 * 7;
            ctx.Context.Response.Headers["Cache-Control"] = "public,max-age=" + duration;
            ctx.Context.Response.Headers["Expires"] = DateTime.UtcNow.AddSeconds(duration).ToString("R");
        }
    });
}

app.UseRouting();
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
