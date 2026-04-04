using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using ReQuest_backend.Server.Auth;
using ReQuest_backend.Server;
using ReQuest_backend.Server.QuestSession;
using ReQuest_backend.Web.DTO;

namespace ReQuest_backend.Web;

[Route("api/game")]
[ApiController]
public class GameController : ControllerBase
{
    private readonly QuestService _questService;
    private readonly GameSessionStore _gameSessionStore;
    private readonly AuthTokenService _authTokenService;

    public GameController(QuestService questService, GameSessionStore gameSessionStore, AuthTokenService authTokenService)
    {
        _questService = questService;
        _gameSessionStore = gameSessionStore;
        _authTokenService = authTokenService;
    }

    [HttpPost("create")]
    public async Task Create([FromBody] CreateGameRequest request, CancellationToken cancellationToken)
    {
        if (!TryGetAuthenticatedUser(out var authUser))
        {
            Response.StatusCode = StatusCodes.Status401Unauthorized;
            await WriteSseEvent("error", new { message = "Сначала войдите или зарегистрируйтесь." }, cancellationToken);
            return;
        }

        Response.StatusCode = 200;
        Response.ContentType = "text/event-stream";

        var questionIds = new System.Collections.Generic.List<long>();

        await foreach (var generation in _questService.CreateNewQuestionsStream(
                           request.Count,
                           request.Difficulty,
                           request.Choice,
                           cancellationToken
                       ))
        {
            if (generation.Stage == "progress" && generation.QuestionId.HasValue)
            {
                questionIds.Add(generation.QuestionId.Value);
            }

            await WriteSseEvent("progress", new
            {
                stage = generation.Stage,
                created = generation.Created,
                total = generation.Total,
                message = generation.Message,
                questionText = generation.QuestionText,
                category = generation.Category
            }, cancellationToken);

            if (generation.Stage == "error")
            {
                await WriteSseEvent("error", new { message = generation.Message }, cancellationToken);
                return;
            }
        }

        if (questionIds.Count == 0)
        {
            await WriteSseEvent("error", new { message = "Не удалось сохранить вопросы для игры." }, cancellationToken);
            return;
        }

        var session = _gameSessionStore.Create(
            authUser!.Name,
            questionIds
        );

        await WriteSseEvent("completed", new GameLobbyResponse(
                session.Code,
                session.HostName,
                session.Players,
                session.QuestionIds.Count
            ),
            cancellationToken
        );
    }

    [HttpPost("join")]
    public ActionResult<GameLobbyResponse> Join([FromBody] JoinGameRequest request)
    {
        var session = _gameSessionStore.Join(request.Code.Trim(), request.PlayerName.Trim());
        if (session == null) return NotFound("Игра с таким кодом не найдена.");

        return Ok(new GameLobbyResponse(
            session.Code,
            session.HostName,
            session.Players,
            session.QuestionIds.Count
        ));
    }

    [HttpGet("lobby/{code}")]
    public ActionResult<GameLobbyResponse> GetLobby(string code)
    {
        var session = _gameSessionStore.Get(code.Trim());
        if (session == null) return NotFound("Игра с таким кодом не найдена.");

        return Ok(new GameLobbyResponse(
            session.Code,
            session.HostName,
            session.Players,
            session.QuestionIds.Count
        ));
    }

    private async Task WriteSseEvent(string eventName, object payload, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(payload);
        await Response.WriteAsync($"event: {eventName}\n", cancellationToken);
        await Response.WriteAsync($"data: {json}\n\n", cancellationToken);
        await Response.Body.FlushAsync(cancellationToken);
    }

    private bool TryGetAuthenticatedUser(out AuthTokenService.AuthProfile? profile)
    {
        profile = null;

        if (!Request.Headers.TryGetValue("Authorization", out var headerValues)) return false;

        var headerValue = headerValues.ToString();
        const string bearerPrefix = "Bearer ";

        if (!headerValue.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase)) return false;

        var token = headerValue[bearerPrefix.Length..].Trim();
        return _authTokenService.TryValidateToken(token, out profile);
    }
}
