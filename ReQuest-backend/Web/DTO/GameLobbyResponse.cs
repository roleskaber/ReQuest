using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReQuest_backend.Web.DTO;

public record GameLobbyResponse(
    [property: JsonPropertyName("code")]
    string Code,
    [property: JsonPropertyName("hostName")]
    string HostName,
    [property: JsonPropertyName("players")]
    List<string> Players,
    [property: JsonPropertyName("questionsCount")]
    int QuestionsCount
);
