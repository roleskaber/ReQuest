namespace ReQuest_backend.Server.Auth;

public interface IAuthTokenService
{
    string IssueToken(string name, string email);
    bool TryValidateToken(string token, out AuthProfile? profile);
}
