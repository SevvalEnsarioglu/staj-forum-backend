namespace staj_forum_backend.DTOs.Chat;

public class ChatMessageDto
{
    public int Id { get; set; }
    public string ConversationId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Response { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}


