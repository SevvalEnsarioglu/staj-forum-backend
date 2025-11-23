using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace staj_forum_backend.Models;

[Table("Replies")]
public class Reply
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int TopicId { get; set; }

    [Required]
    [Column(TypeName = "text")]
    public string Content { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string AuthorName { get; set; } = string.Empty;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation property
    [ForeignKey("TopicId")]
    public virtual Topic Topic { get; set; } = null!;
}


