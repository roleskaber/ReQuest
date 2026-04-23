using System.Collections.Generic;

namespace ReQuest_backend.Server.QuestSession;

public interface IGameSessionStore
{
    event GameSessionCreatedEventHandler? SessionCreated;
    event PlayerJoinedGameSessionEventHandler? PlayerJoined;

    GameSession Create(string hostName, List<long> questionIds);
    GameSession? Join(string code, string playerName);
    GameSession? Get(string code);
    GameSessionState? GetState(string code);
    GameSessionState? Start(string code);
    GameSessionState? NextQuestion(string code);
    GameSessionState? Finish(string code);
    GameSessionState? KickPlayer(string code, string playerName);
    GameSessionState? RemoveQuestion(string code, int questionIndex);
    GameSessionState? SubmitAnswer(string code, string playerName, bool isCorrect);
}
