﻿using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using OpenCvSharp;
using OpenAI;
using System.ClientModel;

// define video file and data folder
string videoFile = VideosHelper.GetVideoFilePathFireTruck();
string dataFolderPath = VideosHelper.CreateDataFolder();

//////////////////////////////////////////////////////
/// VIDEO ANALYSIS using OpenCV
//////////////////////////////////////////////////////

// Extract the frames from the video
var video = new VideoCapture(videoFile);
var frames = new List<Mat>();
while (video.IsOpened())
{
    var frame = new Mat();
    if (!video.Read(frame) || frame.Empty())
        break;
    // resize the frame to half of its size if the with is greater than 800
    if (frame.Width > 800)
    {
        Cv2.Resize(frame, frame, new OpenCvSharp.Size(frame.Width / 2, frame.Height / 2));
    }
    frames.Add(frame);
}
video.Release();

//////////////////////////////////////////////////////
/// OPENAI
//////////////////////////////////////////////////////
#pragma warning disable OPENAI001 

var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
var openai_key = config["OPENAI_KEY"];

OpenAIClient openAIClient = new(openai_key);
ChatClient chatClient = openAIClient.GetChatClient("gpt-4o");

List<ChatMessage> messages =
[
    new SystemChatMessage(PromptsHelper.SystemPrompt),
    new UserChatMessage(PromptsHelper.UserPromptDescribeVideo),
];


// create the OpenAI files that represent the video frames
int step = (int)Math.Ceiling((double)frames.Count / PromptsHelper.NumberOfFrames);

// show in the console the total number of frames and the step that neeeds to be taken to get the desired number of frames for the video analysis
Console.WriteLine($"Video total number of frames: {frames.Count}");
Console.WriteLine($"Get 1 frame every [{step}] to get the [{PromptsHelper.NumberOfFrames}] frames for analysis");

for (int i = 0; i < frames.Count; i += step)
{
    // save the frame to the "data/frames" folder
    string framePath = Path.Combine(dataFolderPath, "frames", $"{i}.jpg");
    Cv2.ImWrite(framePath, frames[i]);

    // read the image bytes, create a new image content part and add it to the messages
    var imageContentPart = ChatMessageContentPart.CreateImagePart(
        imageBytes: BinaryData.FromBytes(File.ReadAllBytes(framePath)),
        imageBytesMediaType: "image/jpeg");
    var message = new UserChatMessage(imageContentPart);
    messages.Add(message);
}

// send the messages to the assistant
AsyncCollectionResult<StreamingChatCompletionUpdate> completionUpdates = chatClient.CompleteChatStreamingAsync(messages);

// print the assistant responses
Console.WriteLine($"[Chat Response using OpenAI .NET Library]: ");
await foreach (StreamingChatCompletionUpdate completionUpdate in completionUpdates)
{
    if (completionUpdate.ContentUpdate.Count > 0)
    {
        Console.Write(completionUpdate.ContentUpdate[0].Text);
    }
}
