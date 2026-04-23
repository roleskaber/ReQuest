using System.Collections.Generic;
using System.Threading.Tasks;
using ReQuest_backend.Server.TriviaAPI.DTO;

namespace ReQuest_backend.Server.Database.Quest;

public interface IQuestRepository
{
    Task<List<QuestEntity>> GetAll();
    Task<List<QuestEntity>> GetRandom(int count);
    Task<QuestEntity?> GetById(long id);
    Task<QuestEntity?> Create(Question questionBody);
    Task<QuestEntity?> Update(QuestEntity entity);
    Task<List<QuestEntity>> GetQuestionsByUserWhoAnswered(long id);
}
