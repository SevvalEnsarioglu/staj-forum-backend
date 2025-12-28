using Microsoft.EntityFrameworkCore;
using staj_forum_backend.Models;

namespace staj_forum_backend.Data.Repositories;

public class ForumRepository : IForumRepository
{
    private readonly ApplicationDbContext _context;

    public ForumRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Topic operations
    public async Task<Topic?> GetTopicByIdAsync(int id)
    {
        return await _context.Topics.FindAsync(id);
    }

    public async Task<Topic?> GetTopicWithRepliesAsync(int id)
    {
        return await _context.Topics
            .Include(t => t.Replies)
            .OrderBy(t => t.Id)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Topic>> GetTopicsAsync(int page, int pageSize, string sortBy, string? search)
    {
        var query = _context.Topics.AsQueryable();

        // Search filter
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(t => t.Title.Contains(search));
        }

        // Sorting
        // Sorting
        query = sortBy.ToLower() switch
        {
            "popular" or "most_viewed" => query.OrderByDescending(t => t.ViewCount).ThenByDescending(t => t.Id),
            "least_viewed" => query.OrderBy(t => t.ViewCount).ThenBy(t => t.Id),
            "a_z" => query.OrderBy(t => t.Title),
            "z_a" => query.OrderByDescending(t => t.Title),
            "oldest" => query.OrderBy(t => t.CreatedAt),
            _ => query.OrderByDescending(t => t.CreatedAt) // newest (default)
        };

        // Pagination
        return await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTopicsCountAsync(string? search)
    {
        var query = _context.Topics.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(t => t.Title.Contains(search));
        }

        return await query.CountAsync();
    }

    public async Task<Topic> CreateTopicAsync(Topic topic)
    {
        topic.CreatedAt = DateTime.UtcNow;
        _context.Topics.Add(topic);
        await _context.SaveChangesAsync();
        return topic;
    }

    public async Task<Topic> UpdateTopicAsync(Topic topic)
    {
        topic.UpdatedAt = DateTime.UtcNow;
        _context.Topics.Update(topic);
        await _context.SaveChangesAsync();
        return topic;
    }

    public async Task<bool> DeleteTopicAsync(int id)
    {
        var topic = await GetTopicByIdAsync(id);
        if (topic == null)
            return false;

        _context.Topics.Remove(topic);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task IncrementTopicViewCountAsync(int id)
    {
        var topic = await GetTopicByIdAsync(id);
        if (topic != null)
        {
            topic.ViewCount++;
            await _context.SaveChangesAsync();
        }
    }

    // Reply operations
    public async Task<Reply?> GetReplyByIdAsync(int id)
    {
        return await _context.Replies
            .Include(r => r.Topic)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Reply>> GetRepliesByTopicIdAsync(int topicId, int page, int pageSize, string sortBy)
    {
        var query = _context.Replies
            .Where(r => r.TopicId == topicId);

        // Sorting
        query = sortBy.ToLower() switch
        {
            "newest" => query.OrderByDescending(r => r.CreatedAt),
            _ => query.OrderBy(r => r.CreatedAt) // oldest (default)
        };

        // Pagination
        return await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetRepliesCountByTopicIdAsync(int topicId)
    {
        return await _context.Replies
            .CountAsync(r => r.TopicId == topicId);
    }

    public async Task<Reply> CreateReplyAsync(Reply reply)
    {
        reply.CreatedAt = DateTime.UtcNow;
        _context.Replies.Add(reply);
        await _context.SaveChangesAsync();
        return reply;
    }

    public async Task<Reply> UpdateReplyAsync(Reply reply)
    {
        reply.UpdatedAt = DateTime.UtcNow;
        _context.Replies.Update(reply);
        await _context.SaveChangesAsync();
        return reply;
    }

    public async Task<bool> DeleteReplyAsync(int id)
    {
        var reply = await GetReplyByIdAsync(id);
        if (reply == null)
            return false;

        _context.Replies.Remove(reply);
        await _context.SaveChangesAsync();
        return true;
    }
}


