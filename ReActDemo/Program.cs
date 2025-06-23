#region --References--
using Microsoft.SemanticKernel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
#endregion

// ReAct pattern: Iterative Reason, Act, and Observe  

var builder = Kernel.CreateBuilder();

// Use environment variables or hardcoded values for Azure OpenAI configuration  
var deploymentName = "<gpt_version>";
var endpoint = "<endpoint>";
var apiKey = "<API_KEY>";
var apiVersion = "<VERSION>";

builder.AddAzureOpenAIChatCompletion(deploymentName, endpoint, apiKey, apiVersion: apiVersion);
builder.Services.AddLogging(c => c.AddConsole());

var kernel = builder.Build();

Console.WriteLine("Enter your question:");
string? userInput = Console.ReadLine(); // Allow userInput to be nullable  

if (string.IsNullOrWhiteSpace(userInput))
{
    Console.WriteLine("No input provided. Exiting.");
    return; // Handle null or empty input gracefully  
}

string context = $"Question: {userInput}\n";
bool done = false;
int step = 1;

while (!done && step <= 10) // limit steps to avoid infinite loops  
{
    // Reason step  
    Console.WriteLine("\n ------------Reasoning Prompt ----------------- \n");
    string reasonPrompt = $@"{context}Reason step: What should you do next? If you have enough information, answer with 'Final Answer: <your answer>'. Otherwise, describe your next action as 'Action: <action description>'.";
    Console.WriteLine(reasonPrompt);
    var reasoning = await kernel.InvokePromptAsync(reasonPrompt);
    string reasoningText = reasoning.GetValue<string>()?.Trim() ?? "";
    Console.WriteLine($"\n[Step {step} Reasoning]\n" + reasoningText);
    context += $"Reasoning: {reasoningText}\n";

    if (reasoningText.StartsWith("Final Answer:", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("\n[Final Answer]");
        Console.WriteLine(reasoningText.Substring("Final Answer:".Length).Trim());
        done = true;
        break;
    }
    else if (reasoningText.StartsWith("Action:", StringComparison.OrdinalIgnoreCase))
    {
        // Simulate action and observation  
        Console.WriteLine("\n ------------ Actions ----------------- \n");
        string action = reasoningText.Substring("Action:".Length).Trim();
        string observation = $"Observation: (Simulated result of '{action}')";
        Console.WriteLine($"[Step {step} Action]\n{action}");
        Console.WriteLine($"[Step {step} Observation]\n{observation}");
        context += $"Action: {action}\n{observation}\n";
    }
    else
    {
        // If the model doesn't follow the format, break to avoid confusion  
        Console.WriteLine("[Warning] Unexpected reasoning format. Stopping loop.");
        break;
    }
    step++;
}
if (!done)
{
    Console.WriteLine("\n[Info] Reached step limit or stopped early without a final answer.");
}
