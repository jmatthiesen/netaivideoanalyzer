using Microsoft.Extensions.AI;
using OpenCvSharp;

public class VideoProcessor(IConfiguration config, ILogger logger)
{
    private IConfiguration _config = config;
    private ILogger _logger = logger;

    public List<Microsoft.Extensions.AI.ChatMessage> CreateMessages(string systemPrompt , string userPrompt, IConfiguration config)
    {
        var messages = new List<Microsoft.Extensions.AI.ChatMessage>();

        if (string.IsNullOrEmpty(systemPrompt))
            systemPrompt = config["VideoAnalyzer:systemPrompt"];
        if (string.IsNullOrEmpty(userPrompt))
            userPrompt = config["VideoAnalyzer:userPrompt"];

        return [
            new Microsoft.Extensions.AI.ChatMessage(ChatRole.System, systemPrompt),
            new Microsoft.Extensions.AI.ChatMessage(ChatRole.User, userPrompt),
        ];
    }

    public List<Mat> ExtractVideoFrames(byte[] videoBytes)
    {
        _logger.LogInformation("Extracting video frames");
        // Convert the video bytes to a file
        var videoPath = Path.Combine(Path.GetTempPath(), "video.mp4");
        File.WriteAllBytes(videoPath, videoBytes);

        // Extract the frames from the video
        var video = new VideoCapture(videoPath);
        var frames = new List<Mat>();
        while (video.IsOpened())
        {
            var frame = new Mat();
            if (!video.Read(frame) || frame.Empty())
                break;
            // resize the frame to half of its size
            Cv2.Resize(frame, frame, new OpenCvSharp.Size(frame.Width / 2, frame.Height / 2));
            frames.Add(frame);
        }
        video.Release();

        // delete the video file
        File.Delete(videoPath);

        _logger.LogInformation($"Video file has total of [{frames.Count}] video frames");
        return frames;
    }

    public async Task<string> AnalyzeVideoAsync(List<Mat> videoFrames, int numberOfFrames, List<Microsoft.Extensions.AI.ChatMessage> messages, IChatClient chatClient)
    {
        _logger.LogInformation($"Analyzing video with [{videoFrames.Count}] total frames, processing [{numberOfFrames}] frames for analysing.");
        bool isFirstFrame = true;

        int step = (int)Math.Ceiling((double)videoFrames.Count / numberOfFrames);

        for (int i = 0; i < videoFrames.Count; i += step)
        {
            // convert the frame to a byte array
            var imageByteArray = videoFrames[i].ToBytes(".jpg");

            // if the first frame, save the image to the disc on the folder "images"
            if (isFirstFrame)
            {
                var framePath = Path.Combine(Directory.GetCurrentDirectory(), "images", "frame.jpg");
                Cv2.ImWrite(framePath, videoFrames[i]);
                isFirstFrame = false;
                _logger.LogInformation($"First frame image saved to [{framePath}]");
            }

            // read the image bytes, create a new image content part and add it to the messages
            AIContent aic = new ImageContent(data: imageByteArray, "image/jpeg");
            var message = new Microsoft.Extensions.AI.ChatMessage(ChatRole.User, [aic]);
            messages.Add(message);
            _logger.LogInformation($"Added image content to the message, frame [{i}]");
        }

        // send the messages to the chat client
        _logger.LogInformation($"Sending messages to the chat client");
        var chatResponse = await chatClient.CompleteAsync(chatMessages: messages);
        _logger.LogInformation($"Chat response: {chatResponse.Message.Text}");
        return chatResponse.Message.Text!;
    }
}
