using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using DocuSync.Web.Components;
using DocuSync.Infrastructure.Extensions;
using DocuSync.Infrastructure.Identity.Interfaces;
using DocuSync.Web.Identity;
using DocuSync.Application.Extensions;
using Microsoft.Extensions.Azure;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();

// Azure AD Authentication
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options =>
    {
        builder.Configuration.GetSection("AzureAd").Bind(options);
        options.SignInScheme = "Cookies";
        options.SignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
    });

builder.Services.AddDocuSyncAuthorization();
builder.Services.AddApplicationServices();

builder.Services.AddMudServices();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});


builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserClaimsAccessor, ClaimsUserAccessor>();
builder.Services.AddDocuSyncInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();