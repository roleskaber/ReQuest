using System.Text.Json.Serialization;

namespace ReQuest_backend.Web.DTO;

public class AuthResponse
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}