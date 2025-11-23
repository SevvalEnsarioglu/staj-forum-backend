namespace staj_forum_backend.DTOs.Forum;

public class TopicDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int ViewCount { get; set; }
    public List<ReplyDto> Replies { get; set; } = new();
    public int ReplyCount { get; set; }
}


