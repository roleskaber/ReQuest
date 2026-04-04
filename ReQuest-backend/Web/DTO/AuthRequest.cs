using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ReQuest_backend.Web.DTO;

public class AuthRequest
{
    [JsonPropertyName("name")]
    [Required]
    [MinLength(2)]
    [MaxLength(32)]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    [Required]
    [EmailAddress]
    [MaxLength(64)]
    public string Email { get; set; } = string.Empty;
}