using staj_forum_backend.DTOs.Common;
using staj_forum_backend.DTOs.Forum;

namespace staj_forum_backend.Services;

public interface IForumService
{
    Task<PagedResultDto<TopicDto>> GetTopicsAsync(int page, int pageSize, string sortBy, string? search);
    Task<TopicDetailDto?> GetTopicByIdAsync(int id);
    Task<TopicDto> CreateTopicAsync(CreateTopicDto createTopicDto);
    Task<TopicDto?> UpdateTopicAsync(int id, UpdateTopicDto updateTopicDto);
    Task<bool> DeleteTopicAsync(int id);

    Task<PagedResultDto<ReplyDto>> GetRepliesByTopicIdAsync(int topicId, int page, int pageSize, string sortBy);
    Task<ReplyDto> CreateReplyAsync(int topicId, CreateReplyDto createReplyDto);
    Task<ReplyDto?> UpdateReplyAsync(int id, CreateReplyDto updateReplyDto);
    Task<bool> DeleteReplyAsync(int id);
}


