using staj_forum_backend.DTOs.Chat;

namespace staj_forum_backend.Services;

public interface IChatService
{
    Task<ChatResponseDto> SendMessageAsync(ChatRequestDto chatRequest);
    Task<string> AnalyzeCvAsync(string cvText);
    Task<IEnumerable<ChatMessageDto>> GetChatHistoryAsync(string? conversationId);
    Task<bool> DeleteChatHistoryAsync(string conversationId);
}


