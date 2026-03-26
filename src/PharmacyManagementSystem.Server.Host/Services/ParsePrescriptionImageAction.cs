using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using PharmacyManagementSystem.Server.Prescription;

namespace PharmacyManagementSystem.Server.Host.Services;

public class ParsePrescriptionImageAction(
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration,
    ILogger<ParsePrescriptionImageAction> logger) : IParsePrescriptionImageAction
{
    private const string AnthropicApiUrl = "https://api.anthropic.com/v1/messages";
    private const string Model = "claude-haiku-4-5-20251001";

    private const string Prompt =
        "This is a photo of a handwritten or printed medical prescription. " +
        "Extract every drug / medicine name and its dosage (if visible) from the prescription. " +
        "Return ONLY a JSON array of strings — one entry per drug, e.g. " +
        "[\"Paracetamol 500mg\",\"Amoxicillin 250mg Capsules\"]. " +
        "If you cannot identify any drugs, return an empty JSON array: []. " +
        "Do not include any explanation or extra text outside the JSON array.";

    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<ParsePrescriptionImageAction> _logger = logger;

    public async Task<IReadOnlyList<string>> ParseAsync(
        Stream imageStream,
        string contentType,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(imageStream);

        var apiKey = _configuration["Anthropic:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            _logger.LogWarning("ParsePrescriptionImageAction: Anthropic:ApiKey is not configured. Returning empty list.");
            return [];
        }

        using var ms = new MemoryStream();
        await imageStream.CopyToAsync(ms, cancellationToken).ConfigureAwait(false);
        var base64 = Convert.ToBase64String(ms.ToArray());

        var mediaType = contentType?.Split(';')[0].Trim().ToLowerInvariant() ?? "image/jpeg";
        if (!new[] { "image/jpeg", "image/png", "image/gif", "image/webp" }.Contains(mediaType))
            mediaType = "image/jpeg";

        var requestBody = new
        {
            model = Model,
            max_tokens = 512,
            messages = new[]
            {
                new
                {
                    role = "user",
                    content = new object[]
                    {
                        new
                        {
                            type = "image",
                            source = new { type = "base64", media_type = mediaType, data = base64 }
                        },
                        new { type = "text", text = Prompt }
                    }
                }
            }
        };

        var json = JsonSerializer.Serialize(requestBody);

        using var client = _httpClientFactory.CreateClient("Anthropic");
        using var request = new HttpRequestMessage(HttpMethod.Post, AnthropicApiUrl);
        request.Headers.Add("x-api-key", apiKey);
        request.Headers.Add("anthropic-version", "2023-06-01");
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        HttpResponseMessage response;
        try
        {
            response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ParsePrescriptionImageAction: HTTP request to Anthropic failed.");
            return [];
        }

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogError("ParsePrescriptionImageAction: Anthropic returned {Status}. Body: {Body}",
                response.StatusCode, errorBody);
            return [];
        }

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        return ExtractDrugNames(responseBody);
    }

    private IReadOnlyList<string> ExtractDrugNames(string responseBody)
    {
        try
        {
            var doc = JsonNode.Parse(responseBody);
            var text = doc?["content"]?[0]?["text"]?.GetValue<string>();
            if (string.IsNullOrWhiteSpace(text)) return [];

            var start = text.IndexOf('[');
            var end = text.LastIndexOf(']');
            if (start < 0 || end < start) return [];

            var drugs = JsonSerializer.Deserialize<List<string>>(text[start..(end + 1)]) ?? [];
            return drugs.Where(d => !string.IsNullOrWhiteSpace(d)).Select(d => d.Trim()).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "ParsePrescriptionImageAction: Could not parse Claude response.");
            return [];
        }
    }
}
