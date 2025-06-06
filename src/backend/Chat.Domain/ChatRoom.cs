namespace Chat.Domain;

public class ChatRoom
{
    public long Id { get; set; }
    public required string Title { get; set; }

    public User Owner { get; set; } = null!;
    public List<User> Members { get; set; } = null!;
    public List<Message> Messages { get; set; } = null!;
}
