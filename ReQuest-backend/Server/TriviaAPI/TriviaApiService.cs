using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using ReQuest_backend.Server.TriviaAPI.DTO;
using ReQuest_backend.Server.TriviaAPI.DTO.Enums;

namespace ReQuest_backend.Server.TriviaAPI;

public class TriviaApiService
{
    private readonly HttpClient _httpClient = new HttpClient();

    public async Task<string?> GetToken()
    {
        var response = await _httpClient.GetFromJsonAsync(
            "https://opentdb.com/api_token.php?command=request",
            typeof(TokenResponse)
        );
        if (response is TokenResponse tokenResponse) return tokenResponse.Token;
        return null;
    }

    public async Task<List<QuestionResponse>> GetQuestions(
        int count,
        string token,
        QuestionDifficultyType? difficulty,
        QuestionChoiceType? choiceType
    )
    {
        var query = new Dictionary<string, string?>
        {
            ["amount"] = count.ToString(),
            ["difficulty"] = difficulty?.ToString(),
            ["type"] = choiceType.ToString(),
            ["token"] = token
        };
        var url = QueryHelpers.AddQueryString("https://opentdb.com/api.php", query);
        var response = await _httpClient.GetFromJsonAsync(
            url,
            typeof(TokenResponse)
        );
        if (response is List<QuestionResponse?> questions) return questions;
        return [];
    }
}