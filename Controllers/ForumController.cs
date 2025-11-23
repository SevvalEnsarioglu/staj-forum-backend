using Microsoft.AspNetCore.Mvc;
using staj_forum_backend.DTOs.Common;
using staj_forum_backend.DTOs.Forum;
using staj_forum_backend.Services;

namespace staj_forum_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ForumController : ControllerBase
{
    private readonly IForumService _forumService;
    private readonly ILogger<ForumController> _logger;

    public ForumController(IForumService forumService, ILogger<ForumController> logger)
    {
        _forumService = forumService;
        _logger = logger;
    }

    /// <summary>
    /// Tüm konuları listeler (sayfalama, sıralama ve arama ile)
    /// </summary>
    [HttpGet("topics")]
    [ProducesResponseType(typeof(PagedResultDto<TopicDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResultDto<TopicDto>>> GetTopics(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string sortBy = "newest",
        [FromQuery] string? search = null)
    {
        try
        {
            var result = await _forumService.GetTopicsAsync(page, pageSize, sortBy, search);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting topics");
            return StatusCode(500, new { error = "An error occurred while retrieving topics." });
        }
    }

    /// <summary>
    /// Belirli bir konuyu detaylarıyla birlikte getirir
    /// </summary>
    [HttpGet("topics/{id}")]
    [ProducesResponseType(typeof(TopicDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TopicDetailDto>> GetTopicById(int id)
    {
        try
        {
            var topic = await _forumService.GetTopicByIdAsync(id);
            if (topic == null)
                return NotFound(new { error = "Topic not found", message = $"Topic with id {id} not found." });

            return Ok(topic);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting topic {TopicId}", id);
            return StatusCode(500, new { error = "An error occurred while retrieving the topic." });
        }
    }

    /// <summary>
    /// Yeni bir konu oluşturur
    /// </summary>
    [HttpPost("topics")]
    [ProducesResponseType(typeof(TopicDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TopicDto>> CreateTopic([FromBody] CreateTopicDto createTopicDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var topic = await _forumService.CreateTopicAsync(createTopicDto);
            return CreatedAtAction(nameof(GetTopicById), new { id = topic.Id }, topic);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating topic");
            return StatusCode(500, new { error = "An error occurred while creating the topic." });
        }
    }

    /// <summary>
    /// Bir konuyu günceller
    /// </summary>
    [HttpPut("topics/{id}")]
    [ProducesResponseType(typeof(TopicDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TopicDto>> UpdateTopic(int id, [FromBody] UpdateTopicDto updateTopicDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var topic = await _forumService.UpdateTopicAsync(id, updateTopicDto);
            if (topic == null)
                return NotFound(new { error = "Topic not found", message = $"Topic with id {id} not found." });

            return Ok(topic);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating topic {TopicId}", id);
            return StatusCode(500, new { error = "An error occurred while updating the topic." });
        }
    }

    /// <summary>
    /// Bir konuyu siler
    /// </summary>
    [HttpDelete("topics/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTopic(int id)
    {
        try
        {
            var deleted = await _forumService.DeleteTopicAsync(id);
            if (!deleted)
                return NotFound(new { error = "Topic not found", message = $"Topic with id {id} not found." });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting topic {TopicId}", id);
            return StatusCode(500, new { error = "An error occurred while deleting the topic." });
        }
    }

    /// <summary>
    /// Bir konuya ait yanıtları listeler
    /// </summary>
    [HttpGet("topics/{topicId}/replies")]
    [ProducesResponseType(typeof(PagedResultDto<ReplyDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResultDto<ReplyDto>>> GetReplies(
        int topicId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string sortBy = "oldest")
    {
        try
        {
            var result = await _forumService.GetRepliesByTopicIdAsync(topicId, page, pageSize, sortBy);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting replies for topic {TopicId}", topicId);
            return StatusCode(500, new { error = "An error occurred while retrieving replies." });
        }
    }

    /// <summary>
    /// Bir konuya yeni yanıt ekler
    /// </summary>
    [HttpPost("topics/{topicId}/replies")]
    [ProducesResponseType(typeof(ReplyDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReplyDto>> CreateReply(int topicId, [FromBody] CreateReplyDto createReplyDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var reply = await _forumService.CreateReplyAsync(topicId, createReplyDto);
            return CreatedAtAction(nameof(GetReplies), new { topicId }, reply);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = "Topic not found", message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating reply for topic {TopicId}", topicId);
            return StatusCode(500, new { error = "An error occurred while creating the reply." });
        }
    }

    /// <summary>
    /// Bir yanıtı günceller
    /// </summary>
    [HttpPut("replies/{id}")]
    [ProducesResponseType(typeof(ReplyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReplyDto>> UpdateReply(int id, [FromBody] CreateReplyDto updateReplyDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var reply = await _forumService.UpdateReplyAsync(id, updateReplyDto);
            if (reply == null)
                return NotFound(new { error = "Reply not found", message = $"Reply with id {id} not found." });

            return Ok(reply);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating reply {ReplyId}", id);
            return StatusCode(500, new { error = "An error occurred while updating the reply." });
        }
    }

    /// <summary>
    /// Bir yanıtı siler
    /// </summary>
    [HttpDelete("replies/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteReply(int id)
    {
        try
        {
            var deleted = await _forumService.DeleteReplyAsync(id);
            if (!deleted)
                return NotFound(new { error = "Reply not found", message = $"Reply with id {id} not found." });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting reply {ReplyId}", id);
            return StatusCode(500, new { error = "An error occurred while deleting the reply." });
        }
    }
}


