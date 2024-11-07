var builder = DistributedApplication.CreateBuilder(args);

var chatDeploymentName = "chat";
var aoai = builder.AddAzureOpenAI("openai")
    .AddDeployment(new AzureOpenAIDeployment(chatDeploymentName, "gpt-4o-mini", "2024-07-18", "GlobalStandard", 4));

var apiService = builder.AddProject<Projects.AspireVideoAnalyserBlazor_ApiService>("apiservice")
    .WithReference(aoai)
    .WithEnvironment("AI_ChatDeploymentName", chatDeploymentName);

builder.AddProject<Projects.VideoAnalysisWeb>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
