using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ReQuest_backend.Web.DTO;

public class SubmitAnswerRequest
{
    [JsonPropertyName("code")]
    [Required]
    [RegularExpression("^\\d{6}$")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("playerName")]
    [Required]
    [MinLength(2)]
    [MaxLength(32)]
    public string PlayerName { get; set; } = string.Empty;

    [JsonPropertyName("answer")]
    [Required]
    [MinLength(1)]
    [MaxLength(256)]
    public string Answer { get; set; } = string.Empty;
}
