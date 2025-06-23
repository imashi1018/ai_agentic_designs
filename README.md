# ReflectAgent Workspace

This repository demonstrates three advanced agent patterns using Microsoft Semantic Kernel and Azure OpenAI:

- **Planning Agent**
- **ReAct Agent**
- **Reflect Agent**

## Project Structure

- `PlanningDemo/` — Demonstrates the Planning pattern (step-by-step plan creation and execution)
- `ReActDemo/` — Demonstrates the ReAct pattern (iterative Reason, Act, and Observe)
- `ReflectAgent/` — Demonstrates the Reflect pattern (self-critique and answer improvement)

## How to Run

1. Ensure you have .NET 8 or .NET 9 SDK installed.
2. Set your Azure OpenAI credentials in each project's `Program.cs` (or use environment variables).
3. Build and run the desired project:
   ```sh
   dotnet run --project <ProjectFolder>
   ```

## Example Questions

### Planning Agent
- "How can I organize a successful online workshop for 50 participants?"
- "What steps should I follow to deploy a .NET application to Azure?"
- "Plan a week-long onboarding process for new software developers."

### ReAct Agent
- "How do I troubleshoot a computer that won't turn on?"
- "You are a robot in a maze. How do you find the exit?"
- "How can I prepare a cup of coffee with the ingredients I have?"

### Reflect Agent
- "Write a brief explanation of how blockchain technology works. Reflect and improve the explanation."
- "Draft a professional email requesting feedback on a project. Reflect on the tone and clarity, and suggest improvements."
- "Write a function in C# to check if a number is prime. Reflect on your code and suggest optimizations."

## Requirements
- .NET 8 or .NET 9
- Azure OpenAI resource and API key

## License
This project is for educational and demonstration purposes.
