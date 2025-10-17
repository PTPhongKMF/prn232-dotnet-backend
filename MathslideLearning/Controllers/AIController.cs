using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace MathslideLearning.Controllers
{
    [Route("api/ai")]
    [ApiController]
    public class AIController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public AIController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] AiRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Message))
                return BadRequest(new { response = "⚠️ Message cannot be empty." });

            var payload = new
            {
                model = "llama3",
                prompt = req.Message,
                stream = true // 🔥 quan trọng: Ollama stream từng dòng JSON
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            using var response = await _httpClient.PostAsync("http://localhost:11434/api/generate", content, HttpContext.RequestAborted);
            response.EnsureSuccessStatusCode();

            // Đọc stream trả về
            using var stream = await response.Content.ReadAsStreamAsync(HttpContext.RequestAborted);
            using var reader = new StreamReader(stream);

            var sb = new StringBuilder();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                try
                {
                    var obj = JsonSerializer.Deserialize<OllamaChunk>(line, options);
                    if (!string.IsNullOrEmpty(obj?.Response))
                    {
                        sb.Append(obj.Response);
                    }

                    if (obj?.Done == true)
                        break;
                }
                catch
                {
                    // Dòng JSON lỗi hoặc không đúng format
                    continue;
                }
            }

            return Ok(new { response = sb.ToString().Trim() });
        }

        private class OllamaChunk
        {
            public string? Response { get; set; }
            public bool Done { get; set; }
        }
    }

    public class AiRequest
    {
        public string Message { get; set; } = "";
    }
}
