using Microsoft.AspNetCore.Mvc;
using staj_forum_backend.DTOs.Chat;
using staj_forum_backend.Services;

namespace staj_forum_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly ILogger<ChatController> _logger;

    public ChatController(IChatService chatService, ILogger<ChatController> logger)
    {
        _chatService = chatService;
        _logger = logger;
    }

    /// <summary>
    /// ChatGPT'ye mesaj gönderir ve yanıt alır
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ChatResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ChatResponseDto>> SendMessage([FromBody] ChatRequestDto chatRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var response = await _chatService.SendMessageAsync(chatRequest);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing chat message");
            return StatusCode(500, new { error = "An error occurred while processing your message." });
        }
    }

    /// <summary>
    /// Chat geçmişini getirir (opsiyonel)
    /// </summary>
    [HttpGet("history")]
    [ProducesResponseType(typeof(IEnumerable<ChatMessageDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ChatMessageDto>>> GetChatHistory(
        [FromQuery] string? conversationId = null)
    {
        try
        {
            var history = await _chatService.GetChatHistoryAsync(conversationId);
            return Ok(history);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting chat history");
            return StatusCode(500, new { error = "An error occurred while retrieving chat history." });
        }
    }

    /// <summary>
    /// Chat geçmişini siler (opsiyonel)
    /// </summary>
    [HttpDelete("history/{conversationId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteChatHistory(string conversationId)
    {
        try
        {
            var deleted = await _chatService.DeleteChatHistoryAsync(conversationId);
            if (!deleted)
                return NotFound(new { error = "Chat history not found", message = $"Chat history with conversation id {conversationId} not found." });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting chat history {ConversationId}", conversationId);
            return StatusCode(500, new { error = "An error occurred while deleting chat history." });
        }
    }
}


