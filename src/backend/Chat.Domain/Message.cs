namespace Chat.Domain;

public class Message
{
    public long Id { get; set; }
    public required string Text { get; set; }
    public DateTime SentAt { get; set; }

    public User Sender { get; set; } = null!;
    public ChatRoom Chat { get; set; } = null!;
}
