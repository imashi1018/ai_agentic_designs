#region --References--
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
#endregion

// Planning Pattern: The agent first creates a plan, then executes each step
var builder = Kernel.CreateBuilder();

var deploymentName = "<gpt_version>";
var endpoint = "<endpoint>";
var apiKey = "<API_KEY>";
var apiVersion = "<VERSION>";

builder.AddAzureOpenAIChatCompletion(deploymentName, endpoint, apiKey, apiVersion: apiVersion);
builder.Services.AddLogging(c => c.AddConsole());

var kernel = builder.Build();

Console.WriteLine("Enter your question:");
string? userInput = Console.ReadLine();
if (string.IsNullOrWhiteSpace(userInput))
{
    Console.WriteLine("No input provided. Exiting.");
    return;
}

// Step 1: Ask the model to create a plan
string planPrompt = $@"You are a planning agent. Given the following question, break it down into a numbered list of clear, executable steps.\nQuestion: {userInput}";
var planResult = await kernel.InvokePromptAsync(planPrompt);
string planText = planResult.GetValue<string>()?.Trim() ?? "";
Console.WriteLine("\n\n ------------Generated Plan ----------------- \n\n");
//Console.WriteLine("\n[Generated Plan]\n" + planText);
Console.WriteLine(planText);

// Step 2: Parse the plan into steps
var steps = planText.Split('\n')
    .Where(line => !string.IsNullOrWhiteSpace(line) && char.IsDigit(line.Trim()[0]))
    .Select(line => line.Substring(line.IndexOf('.') + 1).Trim())
    .ToList();

if (steps.Count == 0)
{
    Console.WriteLine("No steps found in the plan. Exiting.");
    return;
}

// Step 3: Execute each step
for (int i = 0; i < steps.Count; i++)
{
    string stepInstruction = steps[i];
    Console.WriteLine("\n ------------Execute Each steps----------------- \n");
    Console.WriteLine($"\n[Executing Step {i + 1}] {stepInstruction}");
    string execPrompt = $@"You are an expert agent. Execute the following step as part of answering the user's question.\nStep: {stepInstruction}";
    var execResult = await kernel.InvokePromptAsync(execPrompt);
    string execText = execResult.GetValue<string>()?.Trim() ?? "";
    Console.WriteLine($"[Step {i + 1} Result]\n{execText}");
}

Console.WriteLine("\n[Plan Execution Complete]")