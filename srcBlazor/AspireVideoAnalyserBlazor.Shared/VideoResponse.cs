public class VideoResponse
{
    public string VideoDescription { get; set; }
    public int TotalFrames { get; set; }
    public int ProcessedFrames { get; set; }
    public string VideoFrame { get; set; }

    public override string ToString()
    {
        return $"VideoDescription: {VideoDescription}, TotalFrames: {TotalFrames}, ProcessedFrames: {ProcessedFrames}, VideoFrame: {VideoFrame}";
    }
}
