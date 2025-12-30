using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace staj_forum_backend.Models;

[Table("Topics")]
public class Topic
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "text")]
    public string Content { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string AuthorName { get; set; } = string.Empty;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    [Required]
    public int ViewCount { get; set; } = 0;

    public int? UserId { get; set; }
    [ForeignKey("UserId")]
    public virtual User? User { get; set; }

    // Navigation property
    public virtual ICollection<Reply> Replies { get; set; } = new List<Reply>();
}
