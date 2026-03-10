using System.Collections.Generic;
using ReQuest_backend.Server.TriviaAPI.DTO;
using ReQuest_backend.Server.Database.UserAnswer;

namespace ReQuest_backend.Server.Database.Quest;

public class QuestEntity
{
    public long Id { get; set; }
    public QuestionResponse? Body { get; set; } = null;
    public ICollection<UserAnswerEntity> UserAnswers { get; set; } = new List<UserAnswerEntity>();
}
