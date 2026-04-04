using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ReQuest_backend.Server.TriviaAPI.DTO.Enums;

namespace ReQuest_backend.Web.DTO;

public class CreateGameRequest
{
    [JsonPropertyName("hostName")]
    [Required]
    [MinLength(2)]
    [MaxLength(32)]
    public string HostName { get; set; } = string.Empty;

    [JsonPropertyName("hostEmail")]
    [Required]
    [EmailAddress]
    [MaxLength(64)]
    public string HostEmail { get; set; } = string.Empty;

    [JsonPropertyName("count")]
    [Range(1, 50)]
    public int Count { get; set; } = 10;

    [JsonPropertyName("choice")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public QuestionChoiceType? Choice { get; set; }

    [JsonPropertyName("difficulty")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public QuestionDifficultyType? Difficulty { get; set; }
}
