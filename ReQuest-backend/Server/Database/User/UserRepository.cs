using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReQuest_backend.Server.Database.Quest;

namespace ReQuest_backend.Server.Database.User;

public class UserRepository(UserContext db)
{
    private UserContext Db => db;

    public async Task<UserEntity> CreateUser(UserEntity user)
    {
        Db.UserEntities.Add(user);
        await Db.SaveChangesAsync();
        return user;
    }

    public async Task<List<QuestEntity>?> GetQuestionsById(long id, QuestionMode mode)
    {
        var user = await Db.UserEntities.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return null;
        
        return mode == QuestionMode.Answered
            ? user.AnsweredQuests
            : user.CreatedQuests;
    }

    public async Task<QuestEntity?> CreateQuestById(long id, QuestEntity quest, QuestionMode mode)
    {
        var user = await Db.UserEntities.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return null;

        if (mode == QuestionMode.Answered) user.AnsweredQuests.Add(quest);
        else user.CreatedQuests.Add(quest);
        await Db.SaveChangesAsync();

        return quest;
    }
}