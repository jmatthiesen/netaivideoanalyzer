# .NET + AI - Video Analyser

[![Open in GitHub Codespaces](https://github.com/codespaces/badge.svg)](https://codespaces.new/Azure-Samples/netaivideoanalyzer)

## Description

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](/LICENSE)
[![Twitter: elbruno](https://img.shields.io/twitter/follow/elbruno.svg?style=social)](https://twitter.com/elbruno)
![GitHub: elbruno](https://img.shields.io/github/followers/elbruno?style=social)

This repository contains a series of samples on how to analyse a video using multimodal Large Language Models, like GPT-4o or GPT-4o-mini. The repository also includes samples of the use of different AI libraries like:

- [Azure AI OpenAI library for .NET.](https://www.nuget.org/packages/Azure.AI.OpenAI) using **Azure OpenAI Services**
- [Microsoft.Extensions.AI.OpenAI](https://www.nuget.org/packages/Microsoft.Extensions.AI.OpenAI) using **OpenAI APIs**
- [Microsoft.Extensions.AI.AzureAIInference](https://www.nuget.org/packages/Microsoft.Extensions.AI.AzureAIInference) to work with **GitHub Models**
- [Microsoft.Extensions.AI.Ollama](https://www.nuget.org/packages/Microsoft.Extensions.AI.Ollama) to work with **Ollama** local models
- [OpenAI .NET API library](https://www.nuget.org/packages/OpenAI) to work with **OpenAI APIs**.

**Note:** We recommend first going through the [Run Sample Projects](#run-sample-projects) before running this app locally, since some of the demos needs credentials for to work properly.

- [Features](#features)
- [Architecture diagram](#architecture-diagram)
- [Getting started](#getting-started)
- [Run Sample Projects](#run-sample-projects)
  - [Video Analysis Sample Projects Overview](#video-analysis-sample-projects-overview)
  - [Run in GitHub CodeSpaces](#run-in-github-codespaces)
  - [Run GitHub Models samples](#run-github-models-samples)
  - [Run OpenAI .NET Library samples](#run-openai-net-library-samples)
  - [Run Azure OpenAI .NET Library samples](#run-azure-openai-net-library-samples)
  - [Run Ollama sample](#run-ollama-sample)
- [Resources](#resources)
- [Video Recordings](#video-recordings)
- [Contributing](#-contributing)
- [Guidance](#guidance)
  - [Costs](#costs)
  - [Security Guidelines](#security-guidelines)
- [Resources](#resources)

## Features

**GitHub CodeSpaces:** This project is designed to be opened in GitHub Codespaces as an easy way for anyone to try out these libraries entirely in the browser.

This is the OpenAI sample running in Codespaces. The sample analyze this video:

![Firetruck leaving the firestation video](./images/02FireTruck.gif)

And return this output:

![This is the OpenAI sample running in Codespaces](./images/05RunSampleCodespaces.png)

There is also a full Aspire solution, including a Blazor FrontEnd application and an API Service to process the video as a reference application to implement this.

![This is the Aspire Blazor demo running in Codespaces](./images/16AspireBlazorRunSample.png)

## Architecture diagram

COMING SOON!

## Getting Started

The sample Console projects are in the folder [./src/Labs/](./src/Labs/).
The full Aspire + Blazor solution is in the folder [./srcBlazor/](./srcBlazor/).

Currently there are labs for:

- [OpenAI .NET SDK](https://devblogs.microsoft.com/dotnet/announcing-the-stable-release-of-the-official-open-ai-library-for-dotnet/)
- [Microsoft AI Extensions](https://devblogs.microsoft.com/dotnet/introducing-microsoft-extensions-ai-preview/)
- [GitHub Models](https://devblogs.microsoft.com/dotnet/using-github-models-and-dotnet-to-build-generative-ai-apps/)
- [Local Image analysis using Ollama](https://ollama.com/blog/vision-models)
- [OpenCV, using OpenCVSharp](https://github.com/shimat/opencvsharp)

## Run sample projects

### Video Analysis Sample Projects Overview

| Library | Project Name | Description |
|--------------|--------------|-------------|
| [OpenAI library for .NET](https://devblogs.microsoft.com/dotnet/announcing-the-stable-release-of-the-official-open-ai-library-for-dotnet/)  | `.\src\ConsoleOpenAI-04-VideoAnalyzer` | Console project  demonstrating the use of the [OpenAI .NET API library](https://www.nuget.org/packages/OpenAI) to work with **OpenAI APIs**. |
| [Azure AI OpenAI library for .NET.](https://www.nuget.org/packages/Azure.AI.OpenAI) | `.\src\ConsoleAOAI-04-VideoAnalyzer` | Console project  demonstrating the use of the stable release of [Azure AI OpenAI library for .NET.](https://www.nuget.org/packages/Azure.AI.OpenAI) using **Azure OpenAI Services** for the video analysis process.|
| [Microsoft.Extensions.AI](https://devblogs.microsoft.com/dotnet/introducing-microsoft-extensions-ai-preview/)  | `.\src\ConsoleMEAI-04-OpenAI` | Console project  demonstrating the use of the [Microsoft.Extensions.AI.OpenAI](https://www.nuget.org/packages/Microsoft.Extensions.AI.OpenAI) using **OpenAI APIs** for the video analysis process. |
| [Microsoft.Extensions.AI](https://devblogs.microsoft.com/dotnet/introducing-microsoft-extensions-ai-preview/)  | `.\src\ConsoleMEAI-05-GitHubModels` | Console project  demonstrating the use of the [Microsoft.Extensions.AI.AzureAIInference](https://www.nuget.org/packages/Microsoft.Extensions.AI.AzureAIInference) to work with **GitHub Models** |
| [Microsoft.Extensions.AI](https://devblogs.microsoft.com/dotnet/introducing-microsoft-extensions-ai-preview/)  | `.\src\ConsoleMEAI-06-Ollama` | Console project  demonstrating the use of the [Microsoft.Extensions.AI.Ollama](https://www.nuget.org/packages/Microsoft.Extensions.AI.Ollama) to work with **Ollama** local models. This sample uses [Llava 7B](https://ollama.com/library/llava) for image analysis and [Phi 3.5](https://ollama.com/library/phi3.5) for Chat completion |
| [Microsoft.Extensions.AI](https://devblogs.microsoft.com/dotnet/introducing-microsoft-extensions-ai-preview/)  | `.\src\ConsoleMEAI-07-AOAI` | Console project  demonstrating the use of the [Microsoft.Extensions.AI.AzureAIInference](https://www.nuget.org/packages/Microsoft.Extensions.AI.AzureAIInference) to work with **Azure OpenAI Services** |
| [Aspire + Blazor Complete Demo](./docs/blazordemo.md))  | `.\srcBlazor\AspireVideoAnalyserBlazor.sln` | Aspire Project with a frontEnd in Blazor and an API service to analyze videos. Work with **GitHub Models** by default, or **Azure OpenAI Services** with Default Credentials or an API Key.  |

### Run in GitHub CodeSpaces

1. Create a new  Codespace using the `Code` button at the top of the repository.

![create Codespace](./images/17CreateCodeSpace.png)

1. The Codespace createion process can take a couple of minutes.

1. Once the Codespace is loaded, it should have all the necessary requirements to run the demo projects.

### Run GitHub Models samples

To run the sample using GitHub Models, located in `./src/ConsoleMEAI-05-GitHubModels`, you must set a GitHub Personal Token Access ([Get your GitHub Personal Access Token](https://github.com/settings/tokens)).

Once you got your PAT, follow this steps.

- Navigating to the sample project folder using the command:

```bash
cd ./src/ConsoleMEAI-05-GitHubModels/
```

- Run the project with the command:

```bash
dotnet run
```

- You can expect an output similar to this one:

![Run Sample using MEAI and GitHub Models](./images/15GHModelsRunSample.png)

Note: Create GitHub Personal Access Token (PAT) [here](https://github.com/settings/tokens).

### Run OpenAI .NET Library samples

To run the sample using OpenAI APIs, located in `./src/ConsoleOpenAI-04-VideoAnalyzer`, you must set your OpenAI Key in a user secret.

- Navigating to the sample project folder using the command:

```bash
cd ./src/ConsoleOpenAI-04-VideoAnalyzer/
```

- Add the user secrets running the command:

```bash
dotnet user-secrets init
dotnet user-secrets set "OPENAI_KEY" "< your key goes here >"
```

- Run the project with the command:

```bash
dotnet run
```

- You can expect an output similar to this one:

![Run Sample using Azure OpenAI .NET library](./images/10AOAIRunSample.png)

### Run Azure OpenAI .NET Library samples

To run the sample using Azure OpenAI Services, located in `./src/ConsoleAOAI-04-VideoAnalyzer`, you must set your Azure OpenAI keys in a user secret.

- Navigating to the sample project folder using the command:

```bash
cd ./src/ConsoleAOAI-04-VideoAnalyzer/
```

- Add the user secrets running the command:

```bash
dotnet user-secrets init
dotnet user-secrets set "AZURE_OPENAI_MODEL" "gpt-4o"
dotnet user-secrets set "AZURE_OPENAI_ENDPOINT" "https://< your service endpoint >.openai.azure.com/"
dotnet user-secrets set "< your key goes here >" 
```

- Run the project with the command:

```bash
dotnet run
```

- You can expect an output similar to this one:

![Run Sample using OpenAI .NET Library](./images/20OpenAIRunSample.png)

### Run Ollama sample

**Instructions Coming soon!**

## Guidance

### Costs

Pricing varies per region and usage, so it isn't possible to predict exact costs for your usage.
The majority of the Azure resources used in this infrastructure are on usage-based pricing tiers.
However, Azure Container Registry has a fixed cost per registry per day.

You can try the [Azure pricing calculator](https://azure.com/e/2176802ea14941e4959eae8ad335aeb5) for the resources:

- Azure OpenAI Service: S0 tier, gpt-4o-mini model. Pricing is based on token count. [Pricing](https://azure.microsoft.com/pricing/details/cognitive-services/openai-service/)

- Azure Container App: Consumption tier with 0.5 CPU, 1GiB memory/storage. Pricing is based on resource allocation, and each month allows for a certain amount of free usage. [Pricing](https://azure.microsoft.com/pricing/details/container-apps/)

- Azure Container Registry: Basic tier. [Pricing](https://azure.microsoft.com/pricing/details/container-registry/)

- Log analytics: Pay-as-you-go tier. Costs based on data ingested. [Pricing](https://azure.microsoft.com/pricing/details/monitor/)

⚠️ To avoid unnecessary costs, remember to take down your app if it's no longer in use,
either by deleting the resource group in the Portal or running `azd down`.

### Security Guidelines

This template uses Azure OpenAI Services with ApiKey and [Managed Identity](https://learn.microsoft.com/entra/identity/managed-identities-azure-resources/overview) for authenticating to the Azure OpenAI service.

## Resources

### Video Recordings

[![.NET AI Community Standup - Build a Video Analyzer with OpenAI .NET Library’s Stable Release](./images/35netaishow.png)](https://www.youtube.com/live/vQr5N2E_6AM?si=Kmtq0QGNVp4OgFMZ)
