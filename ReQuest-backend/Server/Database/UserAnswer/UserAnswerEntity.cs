namespace ReQuest_backend.Server.Database.UserAnswer;

public class UserAnswerEntity
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long QuestionId { get; set; }
    public string Text { get; set; } =  string.Empty;
}