using staj_forum_backend.Models;

namespace staj_forum_backend.Data.Repositories;

public interface IForumRepository
{
    // Topic operations
    Task<Topic?> GetTopicByIdAsync(int id);
    Task<Topic?> GetTopicWithRepliesAsync(int id);
    Task<IEnumerable<Topic>> GetTopicsAsync(int page, int pageSize, string sortBy, string? search);
    Task<IEnumerable<Topic>> GetTopicsByUserIdAsync(int userId, int page, int pageSize);
    Task<int> GetTopicsCountAsync(string? search);
    Task<int> GetTopicsCountByUserIdAsync(int userId);
    Task<Topic> CreateTopicAsync(Topic topic);
    Task<Topic> UpdateTopicAsync(Topic topic);
    Task<bool> DeleteTopicAsync(int id);
    Task IncrementTopicViewCountAsync(int id);

    // Reply operations
    Task<Reply?> GetReplyByIdAsync(int id);
    Task<IEnumerable<Reply>> GetRepliesByTopicIdAsync(int topicId, int page, int pageSize, string sortBy);
    Task<IEnumerable<Reply>> GetRepliesByUserIdAsync(int userId, int page, int pageSize);
    Task<int> GetRepliesCountByTopicIdAsync(int topicId);
    Task<int> GetRepliesCountByUserIdAsync(int userId);
    Task<Reply> CreateReplyAsync(Reply reply);
    Task<Reply> UpdateReplyAsync(Reply reply);
    Task<bool> DeleteReplyAsync(int id);
}
