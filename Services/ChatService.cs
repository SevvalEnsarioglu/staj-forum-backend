using staj_forum_backend.DTOs.Chat;

namespace staj_forum_backend.Services;

public class ChatService : IChatService
{
    private readonly ILogger<ChatService> _logger;
    private readonly IGeminiService _geminiService;

    public ChatService(ILogger<ChatService> logger, IGeminiService geminiService)
    {
        _logger = logger;
        _geminiService = geminiService;
    }

    public async Task<ChatResponseDto> SendMessageAsync(ChatRequestDto chatRequest)
    {
        var conversationId = chatRequest.ConversationId ?? Guid.NewGuid().ToString();
        
        // Use Gemini Service to get AI response
        var aiResponse = await _geminiService.GenerateResponseAsync(chatRequest.Message);

        _logger.LogInformation("Chat message processed for conversation {ConversationId}", conversationId);

        return new ChatResponseDto
        {
            Response = aiResponse,
            ConversationId = conversationId,
            Timestamp = DateTime.UtcNow
        };
    }

    public Task<IEnumerable<ChatMessageDto>> GetChatHistoryAsync(string? conversationId)
    {
        // TODO: Veritabanından chat geçmişi getirilecek
        // Şimdilik boş liste döndürüyoruz
        
        _logger.LogInformation("Chat history requested for conversation {ConversationId}", conversationId);
        
        return Task.FromResult<IEnumerable<ChatMessageDto>>(new List<ChatMessageDto>());
    }

    public Task<bool> DeleteChatHistoryAsync(string conversationId)
    {
        // TODO: Veritabanından chat geçmişi silinecek
        
        _logger.LogInformation("Chat history deletion requested for conversation {ConversationId}", conversationId);
        
        return Task.FromResult(true);
    }
}


