using BerberApp1.Components;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BerberApp1.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.HttpOverrides;
using BerberApp1.Services;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddHubOptions(options =>
    {
        options.MaximumReceiveMessageSize = 5 * 1024 * 1024; // 5 MB
    });

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.Name = ".BerberApp.Auth";
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
})
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    options.CallbackPath = "/signin-google";
    options.Scope.Add("profile");
    options.Events.OnCreatingTicket = async context =>
    {
        var services = context.HttpContext.RequestServices;
        var dbContextFactory = services.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
        using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var identifier = context.Principal.FindFirst(ClaimTypes.NameIdentifier).Value;
        var email = context.Principal.FindFirst(ClaimTypes.Email).Value;
        var name = context.Principal.FindFirst(ClaimTypes.Name).Value;
        var picture = context.Principal.FindFirst("urn:google:picture")?.Value;

        var user = await dbContext.Users.Include(u => u.Salons).FirstOrDefaultAsync(u => u.GoogleId == identifier);

        if (user == null)
        {
            user = new User
            {
                GoogleId = identifier,
                Email = email,
                Name = name,
                PictureUrl = picture
            };
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
        }
         else if (user.Salons.Any())
         {
             // If the user already exists and has a salon, redirect to the management page.
             context.Properties.RedirectUri = $"/employee-timeline/{user.Salons.First().Id}";
         }
    };
});

builder.Services.AddControllers();
builder.Services.AddSingleton<IFileStorageService, FirebaseStorageService>();
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();


app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Get a logger instance
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application is starting up!");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();