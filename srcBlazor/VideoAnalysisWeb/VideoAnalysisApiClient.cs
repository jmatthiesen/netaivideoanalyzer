public class VideoAnalysisApiClient
{
    private readonly HttpClient httpClient;

    public VideoAnalysisApiClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
        httpClient.Timeout = TimeSpan.FromMinutes(5);
    }

    public async Task<VideoResponse?> AnalyzeVideoAsync(VideoRequest videoRequest, ILogger<Index> logger, CancellationToken cancellationToken = default)
    {
        VideoResponse? videoResponse = null;

        logger.LogInformation("Analyzing video {VideoFileName}", videoRequest.VideoFileName);
        logger.LogInformation($"API Service Base Address: {httpClient.BaseAddress}");

        var response = await httpClient.PostAsJsonAsync<VideoRequest>("/AnalyzeVideo", videoRequest, cancellationToken);

        if (response != null) {
            response.EnsureSuccessStatusCode();
            videoResponse = await response.Content.ReadFromJsonAsync<VideoResponse>(cancellationToken: cancellationToken);
        }
        return videoResponse;
    }
}
