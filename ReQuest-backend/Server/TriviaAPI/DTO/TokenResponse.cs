using System.Text.Json.Serialization;

namespace ReQuest_backend.Server.TriviaAPI.DTO;

public record TokenResponse(
    [property: JsonPropertyName("response_code")] 
    int ResponseCode,
    [property: JsonPropertyName("response_message")] 
    string ResponseMessage,
    [property: JsonPropertyName("token")] 
    string Token
);