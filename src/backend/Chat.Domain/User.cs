namespace Chat.Domain;

public class User
{
    public long Id { get; set; }
    public required string Nickname { get; set; }
    public required string PasswordHash { get; set; }

    public List<ChatRoom> Chats { get; set; } = null!;
}