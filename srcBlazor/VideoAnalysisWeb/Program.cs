using Radzen;
using VideoAnalysisWeb.Components;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRadzenComponents();
builder.Services.AddScoped<Radzen.DialogService>();
builder.Services.AddScoped<Radzen.NotificationService>();
builder.Services.AddScoped<Radzen.ThemeService>();
builder.Services.AddScoped<RadzenComponent>();
builder.Services.AddScoped<Radzen.FileInfo>();

builder.Services.AddHttpClient<VideoAnalysisApiClient>(client =>
{
    // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
    // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
    client.BaseAddress = new("https+http://apiservice");
});
   

var app = builder.Build();

app.MapGet("/SystemInfo", async (ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation("Get System information");
        var systemInfo = await SystemInformation.GetSystemInfoAsync();
        logger.LogInformation("System information retrieved successfully");

        return Results.Json(systemInfo);
    }
    catch (Exception exc)
    {
        logger.LogError(exc, "Error retrieving system information");
        return Results.Problem("Error retrieving system information");
    }
});

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
