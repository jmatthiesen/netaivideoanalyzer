using OpenAI;
using OpenAI.Chat;
using OpenCvSharp;


public class VideoProcessor(IConfiguration config, ILogger logger, ChatClient chatClient)
{
    private readonly IConfiguration config = config;
    private readonly ILogger logger = logger;
    private readonly ChatClient chatClient = chatClient;

    public List<ChatMessage> CreateMessages(string systemPrompt, string userPrompt)
    {
        var messages = new List<ChatMessage>();

        if (string.IsNullOrEmpty(systemPrompt))
            systemPrompt = config["VideoAnalyzer:systemPrompt"]!;
        if (string.IsNullOrEmpty(userPrompt))
            userPrompt = config["VideoAnalyzer:userPrompt"]!;

        return [
            new SystemChatMessage(systemPrompt),
            new UserChatMessage(userPrompt),
        ];
    }

    public List<Mat> ExtractVideoFrames(byte[] videoBytes)
    {
        logger.LogInformation("Extracting video frames");
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

        logger.LogInformation($"Video file has total of [{frames.Count}] video frames");
        return frames;
    }

    public string AnalyzeVideoAsync(List<Mat> videoFrames, int numberOfFrames, List<ChatMessage> messages, ChatClient chatClient)
    {
        logger.LogInformation($"Analyzing video with [{videoFrames.Count}] total frames, processing [{numberOfFrames}] frames for analysing.");
        bool isFirstFrame = true;

        int step = (int)Math.Ceiling((double)videoFrames.Count / numberOfFrames);

        for (int i = 0; i < videoFrames.Count; i += step)
        {
            // convert the frame to a byte array
            var imageByteArray = videoFrames[i].ToBytes(".jpg");

            // if the first frame, save the image to the disc on the folder "images"
            if (isFirstFrame)
            {
                SaveFirstFrame(videoFrames[i]);
                isFirstFrame = false;
            }

            // read the image bytes, create a new image content part and add it to the messages
            var imageContentPart = ChatMessageContentPart.CreateImagePart(
                imageBytes: BinaryData.FromBytes(imageByteArray),
                imageBytesMediaType: "image/jpeg");
            var message = new UserChatMessage(imageContentPart);
            messages.Add(message);
            logger.LogInformation($"Added image content to the message, frame [{i}]");
        }

        //var chatResponse = await _chatClient.CompleteChatAsync(messages: messages);
        try
        {
            // send the messages to the chat client
            logger.LogInformation($"Sending messages to the chat client [CompleteChat]");
            ChatCompletion chatResponse = chatClient.CompleteChat(messages: messages);
            logger.LogInformation($"Chat response: {chatResponse.Content[0].Text}");
            return chatResponse.Content[0].Text!;
        }
        catch (Exception exc)
        {
            logger.LogError($"Error completing chat: {exc.Message}");
            throw;
        }
    }

    private void SaveFirstFrame(Mat videoFrame)
    {
        try
        {
            logger.LogInformation("Saving the first frame image to the disc");
            var framePath = Path.Combine(Directory.GetCurrentDirectory(), "images", "frame.jpg");
            Cv2.ImWrite(framePath, videoFrame);
            logger.LogInformation($"First frame image saved to [{framePath}]");

        }
        catch (Exception ex)
        {
            logger.LogError($"Error saving the first frame image: {ex.Message}");
        }
    }
}
