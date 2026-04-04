using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ReQuest_backend.Server.TriviaAPI.DTO.Enums;

namespace ReQuest_backend.Web.DTO;

public class CreateQuestionRequest
{
    [JsonPropertyName("count")]
    [Range(1, 50)]
    public int Count { get; set; }

    [JsonPropertyName("choice")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public QuestionChoiceType? Choice { get; set; }

    [JsonPropertyName("difficulty")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public QuestionDifficultyType? Difficulty { get; set; }
}