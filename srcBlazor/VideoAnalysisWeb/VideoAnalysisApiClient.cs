public class VideoAnalysisApiClient
{
    private readonly HttpClient httpClient;

    public VideoAnalysisApiClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
        httpClient.Timeout = TimeSpan.FromMinutes(5);
    }

    internal async Task<VideoResponse?> AnalyzeVideoAsync(VideoRequest videoRequest, ILogger<Program> logger, CancellationToken cancellationToken = default)
    {
        VideoResponse? videoResponse = null;

        logger.LogInformation("Analyzing video {VideoFileName}", videoRequest.VideoFileName);
        logger.LogInformation($"API Service Base Address: {httpClient.BaseAddress}");

        var response = await httpClient.PostAsJsonAsync<VideoRequest>("/AnalyzeVideo", videoRequest, cancellationToken);

        if (response != null)
        {
            response.EnsureSuccessStatusCode();
            videoResponse = await response.Content.ReadFromJsonAsync<VideoResponse>(cancellationToken: cancellationToken);
            videoResponse.VideoFrame = httpClient.BaseAddress.AbsoluteUri + videoResponse.VideoFrame;
        }


        logger.LogInformation($"Video Response: {videoResponse}");

        return videoResponse;
    }
}
