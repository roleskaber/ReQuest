using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ReQuest_backend.Web.DTO;

public class JoinGameRequest
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
}
