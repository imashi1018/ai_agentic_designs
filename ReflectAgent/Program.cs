#region --References--
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
#endregion

var builder = Kernel.CreateBuilder();

// Use environment variables for Azure OpenAI configuration
var deploymentName = "<gpt_version>";
var endpoint = "<endpoint>";
var apiKey = "<API_KEY>";
var apiVersion = "<VERSION>";

builder.AddAzureOpenAIChatCompletion(deploymentName, endpoint, apiKey, apiVersion: apiVersion);
builder.Services.AddLogging(c => c.AddConsole());

var kernel = builder.Build();

// Step 1: User Input
Console.WriteLine("Enter your question:");
string? userInput = null;

while (userInput is null || userInput.Trim() == string.Empty)
{
    userInput = Console.ReadLine()?.Trim();
    if (string.IsNullOrEmpty(userInput))
    {
        Console.WriteLine("\n\nPlease enter a valid question:");
    }
}

// Step 2: Think (initial reasoning)
string initialPrompt = $@"
You are an AI assistant. Think step by step about the user's question:
{userInput}
";

Console.WriteLine("\n\n ------------ Initial Prompt ----------------- \n\n");
Console.WriteLine(initialPrompt);


var initialThought = await kernel.InvokePromptAsync(initialPrompt);

Console.WriteLine("\n\n ------------ Thought Process Response  ----------------- \n\n");
Console.WriteLine("\n[Thought Process]\n" + initialThought.GetValue<string>());

// Step 3: Act (simulate a tool - we'll just echo or add something)
string toolResult = $"Tool says: Based on reasoning '{initialThought.GetValue<string>()}'.";

// Step 4: Reflect (check quality of action)
string reflectionPrompt = $@"
You are an AI assistant. Review the following answer for accuracy and completeness. 
If you can improve it, provide a better version.\nQuestion: {userInput}\nAnswer Given: {toolResult}";

try
{
    Console.WriteLine("\n\n ------------Reflection Prompt ----------------- \n\n");
    Console.WriteLine(reflectionPrompt);

    var reflection = await kernel.InvokePromptAsync(reflectionPrompt);
    Console.WriteLine("\n\n ------------ Reflection Response ----------------- \n\n");

    Console.WriteLine("\n[Reflection]\n" + reflection.GetValue<string>());

    // Step 5: Done
    Console.WriteLine("\n\n ------------ Final Response ----------------- \n\n");

    Console.WriteLine("\n[ Final Response to User]");
    Console.WriteLine(reflection.GetValue<string>());
}
catch (Exception ex)
{
    Console.WriteLine("\n[Reflection]\nYour input or prompt was blocked by Azure OpenAI's content filter or caused an error. Please try a different question or rephrase your input.");
}
