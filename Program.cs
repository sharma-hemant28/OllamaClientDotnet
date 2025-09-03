using System.Text.Json;
using System.Net.Http.Json;
using System.Text;

namespace OllamaClientStream
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Ollama Client Streaming Example");
            Console.WriteLine("--------------------------------");

            while (true)
            {
                Console.Write("Enter your prompt (type '/exit', '/bye', or press Ctrl+D to quit): ");
                string? prompt = Console.ReadLine();

                if (prompt == null ||
                    string.Equals(prompt.Trim(), "/exit", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(prompt.Trim(), "/bye", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Exiting. Goodbye!");
                    break;
                }

                if (string.IsNullOrWhiteSpace(prompt))
                {
                    Console.WriteLine("Prompt cannot be empty. Please enter a valid prompt.");
                    continue;
                }

                await SendPromptToOllamaAsync(prompt);
            }
        }

        static async Task SendPromptToOllamaAsync(string prompt)
        {
            Console.WriteLine("Sending request to Ollama local server...");

            var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(10)
            };

            var apiUrl = "http://localhost:11434/api/generate";

            var requestBody = new
            {
                model = "llama3.2:1b",
                prompt = prompt,
                stream = true
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await httpClient.PostAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    using var stream = await response.Content.ReadAsStreamAsync();
                    using var reader = new StreamReader(stream);

                    Console.WriteLine("Streaming response:");

                    string? line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            try
                            {
                                var streamResponse = JsonSerializer.Deserialize<OllamaStreamResponse>(line);
                                if (streamResponse != null)
                                {
                                    if (!string.IsNullOrEmpty(streamResponse.response))
                                        Console.Write(streamResponse.response);

                                    if (streamResponse.done)
                                    {
                                        Console.WriteLine("\n\nStream completed.");
                                        break;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"[Deserialization error]: {ex.Message}");
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"HTTP error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }

    public class OllamaStreamResponse
    {
        public string model { get; set; }
        public DateTime created_at { get; set; }
        public string response { get; set; }
        public bool done { get; set; }
    }
}