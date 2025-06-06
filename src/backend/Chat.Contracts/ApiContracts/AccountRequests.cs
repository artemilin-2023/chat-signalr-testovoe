namespace Chat.Contracts.ApiContracts;

public record RegisterRequest(string Nickname, string Password);

public record LoginRequest(string Nickname, string Password);
