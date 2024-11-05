using Microsoft.Extensions.AI;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddConsole());
builder.Logging.AddConsole();

// add an instance of VideoProcessor
builder.Services.AddSingleton<VideoProcessor>(serviceProvider => {
    return new VideoProcessor(serviceProvider.GetService<IConfiguration>()!, serviceProvider.GetRequiredService<ILogger<Program>>()!);
    });
builder.Services.AddSingleton<IChatClient>(
    serviceProvider => {
        var ChatClient = new ChatClient(serviceProvider.GetService<IConfiguration>()!, serviceProvider.GetRequiredService<ILogger<Program>>()!);
        return ChatClient.GetChatClient();
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

// create a new endpoint that receives a VideoRequest and returns a VideoResponse
app.MapPost("/AnalyzeVideo", async (VideoRequest request, VideoProcessor videoProcessor, IChatClient chatClient, ILogger<Program> logger ) =>
{
    if (request.NumberOfFramesToBeProcessed <= 1)
        request.NumberOfFramesToBeProcessed = 10;

    List<ChatMessage> messages = videoProcessor.CreateMessages(request.SystemPrompt, request.UserPrompt, app.Configuration);

    // extract the frames from the video
    var frames = videoProcessor.ExtractVideoFrames(request.VideoBytes);

    // process the frames
    var processedFrames =  await videoProcessor.AnalyzeVideoAsync(frames, request.NumberOfFramesToBeProcessed, messages, chatClient);

    // create a response
    var response = new VideoResponse
    {
        ProcessedFrames = request.NumberOfFramesToBeProcessed,
        TotalFrames= frames.Count,
        VideoDescription = processedFrames,
        VideoFrame = "/images/frame.jpg"
    };

    // define the complete url for the video frame using the current app running url
    response.VideoFrame = $"{app.Urls.First()}/images/frame.jpg";

    return response;
});

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

app.MapDefaultEndpoints();

app.Run();
