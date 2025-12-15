using System.Text;
using System.Text.Json;
using FitnessCenter.Web.Models.Ai;
using FitnessCenter.Web.Options;
using Microsoft.Extensions.Options;

namespace FitnessCenter.Web.Services
{
    public class GoogleAiService
    {
        private readonly HttpClient _http;
        private readonly GoogleAiOptions _opt;

        public GoogleAiService(HttpClient http, IOptions<GoogleAiOptions> opt)
        {
            _http = http;
            _opt = opt.Value;
        }

        public async Task<AiWorkoutPlan> GeneratePlanAsync(string prompt, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(_opt.ApiKey))
                throw new InvalidOperationException("GoogleAI ApiKey boş. User-Secrets ayarlamalısın.");

            var url =
                $"https://generativelanguage.googleapis.com/v1beta/models/{_opt.Model}:generateContent?key={_opt.ApiKey}";

            var body = new
            {
                contents = new[]
                {
                    new {
                        role = "user",
                        parts = new[] { new { text = prompt } }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.4,
                    responseMimeType = "application/json"
                }
            };

            var json = JsonSerializer.Serialize(body);
            var resp = await _http.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"), ct);
            var respText = await resp.Content.ReadAsStringAsync(ct);

            if (!resp.IsSuccessStatusCode)
                throw new InvalidOperationException($"Gemini hata: {resp.StatusCode}\n{respText}");

            // Gemini response -> candidates[0].content.parts[0].text = JSON string
            using var doc = JsonDocument.Parse(respText);

            var textJson =
                doc.RootElement
                   .GetProperty("candidates")[0]
                   .GetProperty("content")
                   .GetProperty("parts")[0]
                   .GetProperty("text")
                   .GetString();

            if (string.IsNullOrWhiteSpace(textJson))
                throw new InvalidOperationException("Gemini boş cevap döndü.");

            // Plan JSON parse
            var plan = JsonSerializer.Deserialize<AiWorkoutPlan>(textJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (plan == null)
                throw new InvalidOperationException("Plan JSON parse edilemedi.");

            return plan;
        }
    }
}
