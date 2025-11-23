using Microsoft.AspNetCore.Mvc;
using staj_forum_backend.DTOs.Common;
using staj_forum_backend.DTOs.Contact;
using staj_forum_backend.Services;

namespace staj_forum_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;
    private readonly ILogger<ContactController> _logger;

    public ContactController(IContactService contactService, ILogger<ContactController> logger)
    {
        _contactService = contactService;
        _logger = logger;
    }

    /// <summary>
    /// Yeni bir iletişim mesajı gönderir
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ContactMessageDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ContactMessageDto>> CreateContactMessage([FromBody] CreateContactDto createContactDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var message = await _contactService.CreateContactMessageAsync(createContactDto);
            return CreatedAtAction(nameof(GetContactMessageById), new { id = message.Id }, message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating contact message");
            return StatusCode(500, new { error = "An error occurred while sending the message." });
        }
    }

    /// <summary>
    /// Tüm iletişim mesajlarını listeler (admin için)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<ContactMessageDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResultDto<ContactMessageDto>>> GetContactMessages(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] bool? isRead = null)
    {
        try
        {
            var result = await _contactService.GetContactMessagesAsync(page, pageSize, isRead);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting contact messages");
            return StatusCode(500, new { error = "An error occurred while retrieving messages." });
        }
    }

    /// <summary>
    /// Belirli bir iletişim mesajını getirir (admin için)
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ContactMessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContactMessageDto>> GetContactMessageById(int id)
    {
        try
        {
            var message = await _contactService.GetContactMessageByIdAsync(id);
            if (message == null)
                return NotFound(new { error = "Message not found", message = $"Contact message with id {id} not found." });

            return Ok(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting contact message {MessageId}", id);
            return StatusCode(500, new { error = "An error occurred while retrieving the message." });
        }
    }

    /// <summary>
    /// Bir iletişim mesajını okundu olarak işaretler (admin için)
    /// </summary>
    [HttpPut("{id}/read")]
    [ProducesResponseType(typeof(ContactMessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContactMessageDto>> MarkAsRead(int id)
    {
        try
        {
            var message = await _contactService.MarkAsReadAsync(id);
            return Ok(message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = "Message not found", message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking message as read {MessageId}", id);
            return StatusCode(500, new { error = "An error occurred while updating the message." });
        }
    }

    /// <summary>
    /// Bir iletişim mesajını siler (admin için)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteContactMessage(int id)
    {
        try
        {
            var deleted = await _contactService.DeleteContactMessageAsync(id);
            if (!deleted)
                return NotFound(new { error = "Message not found", message = $"Contact message with id {id} not found." });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting contact message {MessageId}", id);
            return StatusCode(500, new { error = "An error occurred while deleting the message." });
        }
    }
}


