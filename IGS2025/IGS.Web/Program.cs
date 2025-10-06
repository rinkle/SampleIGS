using Globalsetting;
using IGS.Dal.Data;  // ✅ ensure this is here
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

var builder = WebApplication.CreateBuilder(args);

// 🔗 Connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// ✅ Correct EF Core registration (no custom hacks)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        connectionString,
        sqlOptions => sqlOptions.CommandTimeout(180) // timeout in seconds
    ));

// 🔐 Identity setup
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()   // ✅ uses your ApplicationDbContext
    .AddDefaultTokenProviders();

// 🔐 Identity cookie config → ensure redirect goes to /igsadmin
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = new PathString("/igsadmin");
    options.LogoutPath = new PathString("/Identity/Account/Logout");
    options.AccessDeniedPath = new PathString("/igsadmin");
});

// 🛠 Dev helpers
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

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

// Home Services
builder.Services.AddScoped<IHomeVmService, HomeVmService>();
builder.Services.AddScoped<IHomeService, HomeService>();

// Transaction Services
builder.Services.AddScoped<ITransactionServicesVmService, TransactionServicesVmService>();
builder.Services.AddScoped<ITransactionServicesService, TransactionServicesService>();

// Portfolio Services
builder.Services.AddScoped<IPortfolioServicesVmService, PortfolioServicesVmService>();
builder.Services.AddScoped<IPortfolioServicesService, PortfolioServicesService>();

// Industry
builder.Services.AddScoped<IIndustryCategoryService, IndustryCategoryService>();
builder.Services.AddScoped<IIndustryService, IndustryService>();
builder.Services.AddScoped<IIndustryVmService, IndustryVmService>();

// Experience
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

// 🔥 Global error handler
app.Use(async (context, next) =>
{
    try { await next(); }
    catch (Exception ex)
    {
        using var scope = app.Services.CreateScope();
        var loggerService = scope.ServiceProvider.GetRequiredService<ILoggerService>();
        await loggerService.LogErrorAsync(ex);
        throw;
    }
});

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
