using Azure.Identity;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Graph.ExternalConnectors;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using MyWebAppTest;
using MyWebAppTest.Models;
using MyWebAppTest.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationInsightsTelemetry();

var initialScopes = builder.Configuration.GetValue<string>("DownstreamApi:Scopes")?.Split(' ');
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
        .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
            .AddMicrosoftGraph(builder.Configuration.GetSection("DownstreamApi"))
            .AddInMemoryTokenCaches();

string appConfigEndpoint = builder.Configuration["appConfigEndpoint"];
builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(new Uri(appConfigEndpoint), new DefaultAzureCredential())
        .ConfigureKeyVault(kv =>
        {
            kv.SetCredential(new DefaultAzureCredential());
        });
});
builder.Services.Configure<Settings>(builder.Configuration.GetSection("TestApp:Settings"));

builder.Services.AddSingleton<ICosmosDbService>
    (CosmosDbRegistration.InitializeCosmosClientInstanceAsync(builder.Configuration.GetSection("CosmosDb")).GetAwaiter().GetResult());

builder.Services.AddScoped<ITableStorageService, TableStorageService>();
builder.Services.AddScoped(typeof(IQueueStorageService<>), typeof(QueueStorageService<>));
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser().Build();

    options.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddRazorPages().AddMicrosoftIdentityUI();

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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});

app.Run();
