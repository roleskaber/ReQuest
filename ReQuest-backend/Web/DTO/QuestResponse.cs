using System.Collections.Generic;
using System.Text.Json.Serialization;
using ReQuest_backend.Server.Database.Quest;
using ReQuest_backend.Server.TriviaAPI.DTO.Enums;

namespace ReQuest_backend.Web.DTO;

public record QuestResponse(
    [property: JsonPropertyName("id")]
    long Id,
    [property: JsonPropertyName("category")]
    string Category,
    [property: JsonPropertyName("question")]
    string Question,
    [property: JsonPropertyName("correctAnswer")]
    string CorrectAnswer,
    [property: JsonPropertyName("incorrectAnswers")]
    List<string> IncorrectAnswers,
    [property: JsonPropertyName("difficulty")]
    QuestionDifficultyType Difficulty,
    [property: JsonPropertyName("type")]
    QuestionChoiceType Type
)
{
    public static QuestResponse FromEntity(QuestEntity entity, List<string> incorrectAnswers)
    {
        return new QuestResponse(
            entity.Id,
            entity.Category,
            entity.QuestionText,
            entity.CorrectAnswer,
            incorrectAnswers,
            entity.Difficulty,
            entity.ChoiceType
        );
    }
}
