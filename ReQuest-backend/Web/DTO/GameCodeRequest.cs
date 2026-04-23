using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ReQuest_backend.Web.DTO;

public class GameCodeRequest
{
    [JsonPropertyName("code")]
    [Required]
    [RegularExpression("^\\d{6}$")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("hostName")]
    [Required]
    [MinLength(2)]
    [MaxLength(32)]
    public string HostName { get; set; } = string.Empty;
}
