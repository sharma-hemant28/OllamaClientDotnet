# Blazor Chatbot with Ollama and LLaMA 3.2:1B

This project is a Blazor WebAssembly application featuring a simple chatbot UI that interacts with a local instance of [Ollama](https://ollama.com). It uses the [LLaMA 3.2:1B](https://ollama.com/library/llama3) model (1.3 GB) downloaded and served locally via Ollama.

The chatbot sends user prompts to the Ollama API (`http://localhost:11434/api/generate`) and streams the AI-generated responses back into the Blazor UI in real time.

---

## 🔧 Features

- ⚡️ Real-time chat interface with streamed responses
- 💬 Local inference using LLaMA 3.2 1B via Ollama
- 🧱 Blazor WebAssembly frontend (C# + Razor)
- 🔌 Connects to local Ollama HTTP API
- 🛠️ Minimal external dependencies

---
## 🚀 Getting Started

1. **Install Ollama** (if not already):

2. Download the LLaMA 3.2 1B model:
ollama pull llama3.2:1b
   
3. Run Ollama locally:
ollama run llama3.2:1b
   
4.Run this Blazor app:
Open in Visual Studio or run: dotnet run

📦 API Reference
This app uses the POST /api/generate endpoint from the Ollama local API to generate responses.
Example payload:
{
  "model": "llama3.2:1b",
  "prompt": "Hello, how are you?",
  "stream": true
}
