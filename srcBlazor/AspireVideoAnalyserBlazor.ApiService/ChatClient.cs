using Azure.AI.Inference;
using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Azure.Identity;

using System.ClientModel;

public class ChatClient(IConfiguration config, ILogger logger)
{
    private IConfiguration _config = config;
    private ILogger _logger = logger;

    public IChatClient GetChatClient()
    {
        var github_token = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
        if (!string.IsNullOrEmpty(github_token))
        {
            _logger.LogInformation("Using GitHub token and GitHub Models.");
            return GetClientFromGitHubModels(github_token);
        }

        var endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
        var modelId = Environment.GetEnvironmentVariable("AZURE_OPENAI_MODEL");
        var apiKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_APIKEY");
        if (!string.IsNullOrEmpty(endpoint) && !string.IsNullOrEmpty(modelId) && !string.IsNullOrEmpty(apiKey))
        {
            _logger.LogInformation("Using AOAI with ApiKey from EnvVars");
            return GetClientFromAOAIApiKey(endpoint, modelId, apiKey);
        }

        if (!string.IsNullOrEmpty(endpoint) && !string.IsNullOrEmpty(modelId))
        {
            _logger.LogInformation("Using AOAI with Default Credentials.");
            return GetClientAOAIUsingDefaultCredentials(endpoint, modelId);
        }

        // default create client from config values
        endpoint = _config["AZURE_OPENAI_ENDPOINT"];
        modelId = _config["AZURE_OPENAI_MODEL"];
        apiKey = _config["AZURE_OPENAI_APIKEY"];
        _logger.LogInformation("Using AOAI with ApiKey from Config Vars");
        return GetClientFromAOAIApiKey(endpoint!, modelId!, apiKey!);
    }

    private IChatClient GetClientFromGitHubModels(string github_token, string modelId = "gpt-4o-mini")
    {
        _logger.LogInformation($"GitHub Models. ModelId: {modelId}");
        IChatClient client = new ChatCompletionsClient(
            endpoint: new Uri("https://models.inference.ai.azure.com"),
            new AzureKeyCredential(github_token))
            .AsChatClient(modelId);
        return client;
    }

    private IChatClient GetClientFromAOAIApiKey(string endpoint, string modelId, string apiKey)
    {
        _logger.LogInformation($"Azure OpenAI with API Key. ModelId: {modelId}. Endpoint {endpoint}");
        var credential = new ApiKeyCredential(apiKey);
        IChatClient chatClient =
            new AzureOpenAIClient(new Uri(endpoint), credential)
            .AsChatClient(modelId: modelId);
        return chatClient;
    }

    private IChatClient GetClientAOAIUsingDefaultCredentials(string endpoint, string modelId)
    {
        _logger.LogInformation($"Azure OpenAI with Default Credentials. ModelId: {modelId}. Endpoint {endpoint}");
        IChatClient client = new AzureOpenAIClient(
            new Uri(endpoint!),
            new DefaultAzureCredential())
            .AsChatClient(modelId: modelId!);
        return client;
    }
}
