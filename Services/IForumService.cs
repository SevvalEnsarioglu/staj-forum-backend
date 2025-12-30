using staj_forum_backend.DTOs.Common;
using staj_forum_backend.DTOs.Forum;

namespace staj_forum_backend.Services;

public interface IForumService
{
    Task<PagedResultDto<TopicDto>> GetTopicsAsync(int page, int pageSize, string sortBy, string? search);
    Task<PagedResultDto<TopicDto>> GetTopicsByUserIdAsync(int userId, int page, int pageSize);
    Task<TopicDetailDto?> GetTopicByIdAsync(int id);
    Task<TopicDto> CreateTopicAsync(CreateTopicDto createTopicDto, int? userId = null);
    Task<TopicDto?> UpdateTopicAsync(int id, UpdateTopicDto updateTopicDto);
    Task<bool> DeleteTopicAsync(int id);

    Task<PagedResultDto<ReplyDto>> GetRepliesByTopicIdAsync(int topicId, int page, int pageSize, string sortBy);
    Task<PagedResultDto<ReplyDto>> GetRepliesByUserIdAsync(int userId, int page, int pageSize);
    Task<ReplyDto> CreateReplyAsync(int topicId, CreateReplyDto createReplyDto, int? userId = null);
    Task<ReplyDto?> UpdateReplyAsync(int id, CreateReplyDto updateReplyDto);
    Task<bool> DeleteReplyAsync(int id);
}
