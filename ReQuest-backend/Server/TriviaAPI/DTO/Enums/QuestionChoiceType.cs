using System.Runtime.Serialization;

namespace ReQuest_backend.Server.TriviaAPI.DTO.Enums;

public enum QuestionChoiceType
{
    [EnumMember(Value = "multiple")]
    Multiple,
    [EnumMember(Value = "boolean")]
    Boolean
}