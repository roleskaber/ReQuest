using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ReQuest_backend.Server.Database.Quest;
using ReQuest_backend.Server.Database.User;
using ReQuest_backend.Server.DTO;
using ReQuest_backend.Server.QuestSession.DTO;
using ReQuest_backend.Server.TriviaAPI.DTO.Enums;

namespace ReQuest_backend.Server;

public interface IQuestService
{
    Task<List<QuestEntity>> CreateNewQuestions(
        int count,
        QuestionDifficultyType? difficulty,
        QuestionChoiceType? choiceType
    );

    IAsyncEnumerable<QuestionGenerationProgress> CreateNewQuestionsStream(
        int count,
        QuestionDifficultyType? difficulty,
        QuestionChoiceType? choiceType,
        CancellationToken cancellationToken = default
    );

    Task<List<QuestEntity>> GetAllQuests();
    Task<StartSessionResponse> StartQuestSession(UserEntity user, int count);
}
