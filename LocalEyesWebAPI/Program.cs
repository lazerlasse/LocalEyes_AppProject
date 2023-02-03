using LocalEyesWebAPI.Data;
using LocalEyesWebAPI.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Logging.
builder.Services.AddLogging();

// Add Data Protection.
builder.Services.AddDataProtection()
    .PersistKeysToDbContext<ApplicationDbContext>();

// Load environment varibles.
builder.Configuration.AddEnvironmentVariables(prefix: "MySpecialSettings_");
var tempPWD = builder.Configuration.GetValue<string>("DbPWD");

// Create connectionstring for the Db.
SqlConnectionStringBuilder mySqlconnection = new(
    builder.Configuration.GetConnectionString("WebAppContextConnection") ?? throw new InvalidOperationException("Connection string 'WebAppContextConnection' not found."))
{
    // Set Passwords in system/Enviroment varibles.
    Password = tempPWD
};

// Create DbContext.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(mySqlconnection.ConnectionString, ServerVersion.AutoDetect(mySqlconnection.ConnectionString)));

// ExeptionHandler for database related error. (Skal fjernes i produktion!!!)
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();


// Configure Identity options.
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz���ABCDEFGHIJKLMNOPQRSTUVWXYZ���0123456789-._@+ ";
    options.User.RequireUniqueEmail = true;
});


builder.Services.AddControllersWithViews(config =>
{
    var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});


// Add Email service.
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(authMessageSenderOptions =>
{
    authMessageSenderOptions.SendGridKey = builder.Configuration.GetValue<string>("SendGridKey");
});


// Add Pushover Service.
builder.Services.AddTransient<PushoverSender>();

// Add Upload File Handler.
builder.Services.AddTransient<UploadFileHandler>();

// Set forwarded headers options.
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseForwardedHeaders();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllers();
});

// Check an user exist.
try
{
    var scope = app.Services.CreateScope();
    await SeedDefaultUser.Initialize(scope.ServiceProvider);
    app.Logger.LogInformation("Standard bruger blev oprettet i databasen.");
}
catch (Exception ex)
{
    app.Logger.LogError(ex.Message, "Der opstod en fejl i forsøget på at oprette standard bruger i databsen!");
}

app.Run();
