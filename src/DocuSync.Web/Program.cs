using DocuSync.Application.Extensions;
using DocuSync.Infrastructure.Identity.Interfaces;
using DocuSync.Web.Components;
using DocuSync.Web.Identity;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using MudBlazor.Services;
using DocuSync.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using DocuSync.Application.Services.Interfaces;
using DocuSync.Infrastructure.Documents;
using DocuSync.Application.Services;
using DocuSync.Domain.Repositories;
using DocuSync.Infrastructure.Data.Repositories;
using DocuSync.Web.ExcelTemp;
using DocuSync.Infrastructure.Portal.Interfaces;
using DocuSync.Infrastructure.Portal.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();

// Add authentication services
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

// Add authorization services
builder.Services.AddDocuSyncAuthorization();

builder.Services.AddMudServices();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserClaimsAccessor, ClaimsUserAccessor>();
builder.Services.AddSingleton<IDocumentStorage, AzureBlobStorage>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IRequirementRepository, RequirementRepository>();
builder.Services.AddScoped<IRequirementService, RequirementService>();
builder.Services.AddScoped<IDocumentTypeRepository, DocumentTypeRepository>();
builder.Services.AddScoped<IDocumentTypeService, DocumentTypeService>();
builder.Services.AddScoped<IPortalService, OrangePortalService>();
//Temp excel
builder.Services.AddSingleton<IExcelDocumentService, ExcelDocumentService>();

// Add infrastructure services
builder.Services.AddDocuSyncInfrastructure(builder.Configuration);
// Add identity services
builder.Services.AddDocuSyncIdentity(builder.Configuration);

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
    .AddInteractiveServerRenderMode()
    .RequireAuthorization();

app.Run();