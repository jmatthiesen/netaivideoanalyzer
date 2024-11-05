public class VideoAnalysisApiClient
{
    private readonly HttpClient httpClient;

    public VideoAnalysisApiClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
        httpClient.Timeout = TimeSpan.FromMinutes(5);
    }

    public async Task<VideoResponse?> AnalyzeVideoAsync(VideoRequest videoRequest, CancellationToken cancellationToken = default)
    {
        VideoResponse? videoResponse = null;

        var response = await httpClient.PostAsJsonAsync<VideoRequest>("/AnalyzeVideo", videoRequest, cancellationToken);

        if (response != null) {
            response.EnsureSuccessStatusCode();
            videoResponse = await response.Content.ReadFromJsonAsync<VideoResponse>(cancellationToken: cancellationToken);
        }
        return videoResponse;
    }
}
