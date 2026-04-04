using System.Collections.Generic;
using ReQuest_backend.Server.Database.Quest;

namespace ReQuest_backend.Server.QuestSession.DTO;

public record StartSessionResponse(
    string Token,
    List<QuestEntity> QuestEntities
);