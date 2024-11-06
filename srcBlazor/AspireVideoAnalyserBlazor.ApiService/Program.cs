using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using OpenAI;
using OpenAI.Chat;
using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddConsole());
builder.Logging.AddConsole();

builder.AddAzureOpenAIClient("openai");

// get chat client from aspire hosting configuration
builder.Services.AddSingleton(serviceProvider =>
{
    var config = serviceProvider.GetService<IConfiguration>()!;
    OpenAIClient client = serviceProvider.GetRequiredService<OpenAIClient>();
    var chatClient = client.GetChatClient(config["AI_ChatDeploymentName"]);
    return chatClient;
});

// add an instance of VideoProcessor
builder.Services.AddSingleton(serviceProvider =>
{
    return new VideoProcessor(
        serviceProvider.GetService<IConfiguration>()!,
        serviceProvider.GetRequiredService<ILogger<Program>>()!,
        serviceProvider.GetService<ChatClient>()!);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

// create a new endpoint that receives a VideoRequest and returns a VideoResponse
app.MapPost("/AnalyzeVideo", async (VideoRequest request, VideoProcessor videoProcessor, ILogger<Program> logger) =>
{
    if (request.NumberOfFramesToBeProcessed <= 1)
        request.NumberOfFramesToBeProcessed = 10;

    List<ChatMessage> messages = videoProcessor.CreateMessages(request.SystemPrompt, request.UserPrompt, app.Configuration);

    // extract the frames from the video
    var frames = videoProcessor.ExtractVideoFrames(request.VideoBytes);

    // process the frames
    var processedFrames = await videoProcessor.AnalyzeVideoAsync(frames, request.NumberOfFramesToBeProcessed, messages);

    // create a response
    var response = new VideoResponse
    {
        ProcessedFrames = request.NumberOfFramesToBeProcessed,
        TotalFrames = frames.Count,
        VideoDescription = processedFrames,
        VideoFrame = "/images/frame.jpg"
    };

    // define the complete url for the video frame using the current app running url
    response.VideoFrame = $"{app.Urls.First()}/images/frame.jpg";

    return response;
});

app.MapGet("/SystemInfo", async (ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation("Get System information");
        var systemInfo = await SystemInfo.GetSystemInfoAsync();
        return Results.Json(systemInfo);
    }
    catch (Exception exc)
    {
        logger.LogError(exc, "Error retrieving system information");
        return Results.Problem("Error retrieving system information");
    }
});

app.MapGet("/", (ILogger<Program> logger) =>
{
    logger.LogInformation("Root call!");
    return Results.Text($"Hello from Aspire Video Analyser API Service. Current date and time is {DateTime.Now}");
});

try
{
    // publish the content of the folder "images", as images
    if (!Directory.Exists("images"))
    {
        Directory.CreateDirectory("images");
    }
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "images")),
        RequestPath = "/images"
    });
}
catch (Exception ex)
{
    app.Logger.LogError(ex, "Error publishing images folder");
}

app.MapDefaultEndpoints();

app.Run();
