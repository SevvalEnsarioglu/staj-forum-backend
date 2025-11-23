namespace staj_forum_backend.DTOs.Chat;

public class ChatResponseDto
{
    public string Response { get; set; } = string.Empty;
    public string ConversationId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}


