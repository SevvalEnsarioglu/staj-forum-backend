using AutoMapper;
using staj_forum_backend.Data.Repositories;
using staj_forum_backend.DTOs.Common;
using staj_forum_backend.DTOs.Forum;
using staj_forum_backend.Models;

namespace staj_forum_backend.Services;

public class ForumService : IForumService
{
    private readonly IForumRepository _forumRepository;
    private readonly IMapper _mapper;

    public ForumService(IForumRepository forumRepository, IMapper mapper)
    {
        _forumRepository = forumRepository;
        _mapper = mapper;
    }

    public async Task<PagedResultDto<TopicDto>> GetTopicsAsync(int page, int pageSize, string sortBy, string? search)
    {
        // Validate pagination parameters
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var topics = await _forumRepository.GetTopicsAsync(page, pageSize, sortBy, search);
        var totalCount = await _forumRepository.GetTopicsCountAsync(search);

        var topicDtos = _mapper.Map<List<TopicDto>>(topics);

        // Get reply counts for each topic
        foreach (var topicDto in topicDtos)
        {
            var replyCount = await _forumRepository.GetRepliesCountByTopicIdAsync(topicDto.Id);
            topicDto.ReplyCount = replyCount;
        }

        return new PagedResultDto<TopicDto>
        {
            Data = topicDtos,
            Pagination = new PaginationDto
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            }
        };
    }

    public async Task<TopicDetailDto?> GetTopicByIdAsync(int id)
    {
        var topic = await _forumRepository.GetTopicWithRepliesAsync(id);
        if (topic == null)
            return null;

        // Increment view count
        await _forumRepository.IncrementTopicViewCountAsync(id);

        var topicDetailDto = _mapper.Map<TopicDetailDto>(topic);
        topicDetailDto.ReplyCount = topic.Replies.Count;

        // Re-fetch to get updated view count
        var updatedTopic = await _forumRepository.GetTopicByIdAsync(id);
        if (updatedTopic != null)
        {
            topicDetailDto.ViewCount = updatedTopic.ViewCount;
        }

        return topicDetailDto;
    }

    public async Task<TopicDto> CreateTopicAsync(CreateTopicDto createTopicDto, int? userId = null)
    {
        var topic = _mapper.Map<Topic>(createTopicDto);
        topic.UserId = userId; // Set optional UserId
        var createdTopic = await _forumRepository.CreateTopicAsync(topic);
        return _mapper.Map<TopicDto>(createdTopic);
    }

    public async Task<TopicDto?> UpdateTopicAsync(int id, UpdateTopicDto updateTopicDto)
    {
        var topic = await _forumRepository.GetTopicByIdAsync(id);
        if (topic == null)
            return null;

        topic.Title = updateTopicDto.Title;
        topic.Content = updateTopicDto.Content;
        topic.UpdatedAt = DateTime.UtcNow;

        var updatedTopic = await _forumRepository.UpdateTopicAsync(topic);
        return _mapper.Map<TopicDto>(updatedTopic);
    }

    public async Task<bool> DeleteTopicAsync(int id)
    {
        return await _forumRepository.DeleteTopicAsync(id);
    }

    public async Task<PagedResultDto<ReplyDto>> GetRepliesByTopicIdAsync(int topicId, int page, int pageSize, string sortBy)
    {
        // Validate pagination parameters
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        // Check if topic exists
        var topic = await _forumRepository.GetTopicByIdAsync(topicId);
        if (topic == null)
        {
            return new PagedResultDto<ReplyDto>
            {
                Data = new List<ReplyDto>(),
                Pagination = new PaginationDto
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = 0,
                    TotalPages = 0
                }
            };
        }

        var replies = await _forumRepository.GetRepliesByTopicIdAsync(topicId, page, pageSize, sortBy);
        var totalCount = await _forumRepository.GetRepliesCountByTopicIdAsync(topicId);

        var replyDtos = _mapper.Map<List<ReplyDto>>(replies);

        return new PagedResultDto<ReplyDto>
        {
            Data = replyDtos,
            Pagination = new PaginationDto
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            }
        };
    }

    public async Task<ReplyDto> CreateReplyAsync(int topicId, CreateReplyDto createReplyDto, int? userId = null)
    {
        // Check if topic exists
        var topic = await _forumRepository.GetTopicByIdAsync(topicId);
        if (topic == null)
            throw new KeyNotFoundException($"Topic with id {topicId} not found");

        var reply = _mapper.Map<Reply>(createReplyDto);
        reply.TopicId = topicId;
        reply.UserId = userId; // Set optional UserId

        var createdReply = await _forumRepository.CreateReplyAsync(reply);
        return _mapper.Map<ReplyDto>(createdReply);
    }

    public async Task<ReplyDto?> UpdateReplyAsync(int id, CreateReplyDto updateReplyDto)
    {
        var reply = await _forumRepository.GetReplyByIdAsync(id);
        if (reply == null)
            return null;

        reply.Content = updateReplyDto.Content;
        reply.AuthorName = updateReplyDto.AuthorName;
        reply.UpdatedAt = DateTime.UtcNow;

        var updatedReply = await _forumRepository.UpdateReplyAsync(reply);
        return _mapper.Map<ReplyDto>(updatedReply);
    }

    public async Task<bool> DeleteReplyAsync(int id)
    {
        return await _forumRepository.DeleteReplyAsync(id);
    }

    public async Task<PagedResultDto<TopicDto>> GetTopicsByUserIdAsync(int userId, int page, int pageSize)
    {
        // Validate pagination parameters
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var topics = await _forumRepository.GetTopicsByUserIdAsync(userId, page, pageSize);
        var totalCount = await _forumRepository.GetTopicsCountByUserIdAsync(userId);

        var topicDtos = _mapper.Map<List<TopicDto>>(topics);

        // Get reply counts for each topic
        foreach (var topicDto in topicDtos)
        {
            var replyCount = await _forumRepository.GetRepliesCountByTopicIdAsync(topicDto.Id);
            topicDto.ReplyCount = replyCount;
        }

        return new PagedResultDto<TopicDto>
        {
            Data = topicDtos,
            Pagination = new PaginationDto
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            }
        };
    }

    public async Task<PagedResultDto<ReplyDto>> GetRepliesByUserIdAsync(int userId, int page, int pageSize)
    {
        // Validate pagination parameters
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var replies = await _forumRepository.GetRepliesByUserIdAsync(userId, page, pageSize);
        var totalCount = await _forumRepository.GetRepliesCountByUserIdAsync(userId);

        var replyDtos = _mapper.Map<List<ReplyDto>>(replies);

        return new PagedResultDto<ReplyDto>
        {
            Data = replyDtos,
            Pagination = new PaginationDto
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            }
        };
    }
}
