using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using ReQuest_backend.Server.TriviaAPI.DTO;

namespace ReQuest_backend.Server.Translation;

public class QuestionTranslationService
{
    private readonly HttpClient _httpClient;
    private readonly Dictionary<string, string> _cache = new();

    public QuestionTranslationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Question> TranslateToRussian(Question question)
    {
        var translatedCategory = await TranslateText(question.Category);
        var translatedQuestion = await TranslateText(question.QuestionText);
        var translatedCorrect = await TranslateText(question.CorrectAnswer);

        List<string> translatedIncorrect = [];
        foreach (var answer in question.IncorrectAnswers)
        {
            translatedIncorrect.Add(await TranslateText(answer));
        }

        return question with
        {
            Category = translatedCategory,
            QuestionText = translatedQuestion,
            CorrectAnswer = translatedCorrect,
            IncorrectAnswers = translatedIncorrect
        };
    }

    private async Task<string> TranslateText(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return input;
        if (_cache.TryGetValue(input, out var cached)) return cached;

        var url = QueryHelpers.AddQueryString(
            "https://translate.googleapis.com/translate_a/single",
            new Dictionary<string, string?>
            {
                ["client"] = "gtx",
                ["sl"] = "auto",
                ["tl"] = "ru",
                ["dt"] = "t",
                ["q"] = input
            }
        );

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode) return input;

        var payload = await response.Content.ReadAsStringAsync();
        var translated = ParseTranslatedText(payload);
        if (string.IsNullOrWhiteSpace(translated)) return input;

        _cache[input] = translated;
        return translated;
    }

    private static string? ParseTranslatedText(string payload)
    {
        using var document = JsonDocument.Parse(payload);
        if (document.RootElement.ValueKind != JsonValueKind.Array || document.RootElement.GetArrayLength() == 0)
            return null;

        var segments = document.RootElement[0];
        if (segments.ValueKind != JsonValueKind.Array) return null;

        List<string> parts = [];
        foreach (var segment in segments.EnumerateArray())
        {
            if (segment.ValueKind != JsonValueKind.Array || segment.GetArrayLength() == 0) continue;
            var piece = segment[0].GetString();
            if (!string.IsNullOrWhiteSpace(piece)) parts.Add(piece);
        }

        return parts.Count == 0 ? null : string.Concat(parts);
    }
}
