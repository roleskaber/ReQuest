namespace ReQuest_backend.Server.DTO;

public record QuestionGenerationProgress(
    string Stage,
    int Created,
    int Total,
    string Message,
    long? QuestionId = null,
    string? QuestionText = null,
    string? Category = null
);
