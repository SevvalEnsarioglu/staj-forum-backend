namespace staj_forum_backend.DTOs.Forum;

public class ReplyDto
{
    public int Id { get; set; }
    public int TopicId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}


