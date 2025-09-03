namespace OllamaClientBlazorApp.Components
{
    public class OllamaStreamResponse
    {
        public string model { get; set; }
        public DateTime created_at { get; set; }
        public string response { get; set; }
        public bool done { get; set; }
    }
}
