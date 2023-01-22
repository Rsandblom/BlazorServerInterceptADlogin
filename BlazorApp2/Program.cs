using BlazorApp2.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


//builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie()
//    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"), OpenIdConnectDefaults.AuthenticationScheme, null);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie()
    .AddMicrosoftIdentityWebApp(options =>
    {
        builder.Configuration.Bind("AzureAd", options);
        //options.Events = new SurveyAuthenticationEvents(loggerFactory);
        //options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //options.Events.OnTokenValidated += options.Events.TokenValidated;
        options.Events = new OpenIdConnectEvents
        {
            OnTokenValidated = ctx =>
            {
                //query the database to get the role

                // add claims
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Admin")
            };
                var appIdentity = new ClaimsIdentity(claims);

                ctx.Principal.AddIdentity(appIdentity);

                //TODO lägg till en service som Principal kan sparas i och skapa en custom Authenticationstateprovider som kan sätta den AD Principalen
                //som User, typ...
                //Testa hämta service med ctx.HttpContext.RequestServices.GetRequiredService<YServicen för o hålla principal>();
                //eller bara kalla på sätta den AD Principalen metoden i custom Authenticationstateprovider.. Custom.Authenticationstateprovider.SetAdUserAsUser(ctx.Principal)

                return Task.CompletedTask;
            },
        };
    }, cookieScheme: null);


    //builder.Configuration.GetSection("AzureAd"), OpenIdConnectDefaults.AuthenticationScheme, null);






builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy
    //options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddMicrosoftIdentityConsentHandler();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
//builder.Services.AddScoped<HttpClient>();

builder.Services.AddControllers();  
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


//app.MapControllers();            // new
//app.MapBlazorHub();              // existing
//app.MapFallbackToPage("/_Host"); // existing

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();            // new
    endpoints.MapBlazorHub();              // existing
    endpoints.MapFallbackToPage("/_Host"); // existing
});

app.Run();
