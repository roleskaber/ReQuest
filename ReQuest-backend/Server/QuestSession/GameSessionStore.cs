using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ReQuest_backend.Server.QuestSession;

public class GameSessionStore
{
    private readonly ConcurrentDictionary<string, GameSession> _sessions = new();
    private readonly Random _random = new();

    public GameSession Create(string hostName, List<long> questionIds)
    {
        var code = GenerateUniqueCode();
        var session = new GameSession
        {
            Code = code,
            HostName = hostName,
            QuestionIds = questionIds,
            Players = [hostName],
            CreatedAt = DateTimeOffset.UtcNow
        };

        _sessions[code] = session;
        return session;
    }

    public GameSession? Join(string code, string playerName)
    {
        if (!_sessions.TryGetValue(code.ToUpperInvariant(), out var session)) return null;

        lock (session)
        {
            var alreadyJoined = session.Players.Any(p => p.Equals(playerName, StringComparison.OrdinalIgnoreCase));
            if (!alreadyJoined) session.Players.Add(playerName);
        }

        return session;
    }

    public GameSession? Get(string code)
    {
        if (_sessions.TryGetValue(code.ToUpperInvariant(), out var session)) return session;
        return null;
    }

    private string GenerateUniqueCode()
    {
        while (true)
        {
            var chars = new char[6];
            for (var i = 0; i < chars.Length; i++)
            {
                chars[i] = (char)('0' + _random.Next(10));
            }

            var code = new string(chars);
            if (!_sessions.ContainsKey(code)) return code;
        }
    }
}

public class GameSession
{
    public required string Code { get; init; }
    public required string HostName { get; init; }
    public required List<string> Players { get; init; }
    public required List<long> QuestionIds { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
}
