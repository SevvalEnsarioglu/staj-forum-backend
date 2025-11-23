using staj_forum_backend.DTOs.Chat;

namespace staj_forum_backend.Services;

public class ChatService : IChatService
{
    private readonly ILogger<ChatService> _logger;
    private readonly IConfiguration _configuration;

    public ChatService(ILogger<ChatService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public Task<ChatResponseDto> SendMessageAsync(ChatRequestDto chatRequest)
    {
        // TODO: OpenAI API entegrasyonu buraya eklenecek
        // Şimdilik mock response döndürüyoruz
        
        var conversationId = chatRequest.ConversationId ?? Guid.NewGuid().ToString();
        
        // Mock response - OpenAI API entegrasyonu yapıldığında bu kısım değişecek
        var mockResponse = $"Staj hakkında sorduğunuz '{chatRequest.Message}' sorusu için: " +
                          "Staj başvurusu yapmak için öncelikle şirketlerin kariyer sayfalarını " +
                          "ziyaret edebilir veya LinkedIn üzerinden başvuru yapabilirsiniz. " +
                          "Staj sürecinde CV'nizi güncel tutmanız ve ilgili deneyimlerinizi " +
                          "vurgulamanız önemlidir.";

        _logger.LogInformation("Chat message processed for conversation {ConversationId}", conversationId);

        return Task.FromResult(new ChatResponseDto
        {
            Response = mockResponse,
            ConversationId = conversationId,
            Timestamp = DateTime.UtcNow
        });
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


