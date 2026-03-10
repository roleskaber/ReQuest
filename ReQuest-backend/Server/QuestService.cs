using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using ReQuest_backend.Server.Database.Quest;
using ReQuest_backend.Server.TriviaAPI;
using ReQuest_backend.Server.TriviaAPI.DTO.Enums;

namespace ReQuest_backend.Server;

public class QuestService
{
    private readonly TriviaApiService _triviaApiService = new TriviaApiService();

    private readonly QuestRepository _questRepository = new QuestRepository(
        new QuestContext()
    );

    public async Task<List<QuestEntity>> CreateNewQuestions(
        int count,
        QuestionChoiceType? choiceType,
        QuestionDifficultyType? difficulty
    )
    {
        var token = await _triviaApiService.GetToken();
        if (token == null) return [];

        var questions = await _triviaApiService.GetQuestions(count, token, difficulty, choiceType);
        List<QuestEntity> questEntities = [];
        foreach (var questionResponse in questions)
        {
            var entity = await _questRepository.Create(questionResponse);
            if (entity != null) questEntities.Add(entity);
        }

        return questEntities;
    }

    public void StartQuestSession()
    {
    }
}