var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.AspireVideoAnalyserBlazor_ApiService>("apiservice");

builder.AddProject<Projects.VideoAnalysisWeb>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
