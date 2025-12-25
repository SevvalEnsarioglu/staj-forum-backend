using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace staj_forum_backend.Services;

public interface IGeminiService
{
    Task<string> GenerateResponseAsync(string prompt);
}

public class GeminiService : IGeminiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private const string BaseUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";

    public GeminiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["GeminiApiKey"];
    }

    public async Task<string> GenerateResponseAsync(string prompt)
    {
        if (string.IsNullOrEmpty(_apiKey))
        {
            return "Gemini API Key is missing. Please configure it in appsettings.json.";
        }

        var requestUrl = $"{BaseUrl}?key={_apiKey}";

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
        };

        var jsonContent = JsonSerializer.Serialize(requestBody);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync(requestUrl, httpContent);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            var geminiResponse = JsonSerializer.Deserialize<GeminiResponseRoot>(responseJson);

            // Extract the text from the response structure
            var text = geminiResponse?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;
            
            return text ?? "I couldn't understand that.";
        }
        catch (HttpRequestException ex)
        {
            return $"Error communicating with AI: {ex.Message}";
        }
    }
}

// Helper classes for JSON Deserialization
public class GeminiResponseRoot
{
    [JsonPropertyName("candidates")]
    public List<Candidate>? Candidates { get; set; }
}

public class Candidate
{
    [JsonPropertyName("content")]
    public Content? Content { get; set; }
}

public class Content
{
    [JsonPropertyName("parts")]
    public List<Part>? Parts { get; set; }
}

public class Part
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }
}
