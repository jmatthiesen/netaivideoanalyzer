public class VideoRequest
{

    public string VideoFileName { get; set; }
    public string VideoFileContentType { get; set; }

    public byte[] VideoBytes { get; set; }
    public int NumberOfFrames { get; set; }

    public int NumberOfFramesToBeProcessed { get; set; }

    public string SystemPrompt { get; set; }
    public string UserPrompt { get; set; }
}
