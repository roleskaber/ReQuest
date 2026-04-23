using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ReQuest_backend.Web.DTO;

public class RemoveQuestionRequest : GameCodeRequest
{
    [JsonPropertyName("questionIndex")]
    [Range(0, 500)]
    public int QuestionIndex { get; set; }
}

