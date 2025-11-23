using System.ComponentModel.DataAnnotations;

namespace staj_forum_backend.DTOs.Forum;

public class UpdateTopicDto
{
    [Required(ErrorMessage = "Başlık zorunludur.")]
    [MinLength(3, ErrorMessage = "Başlık en az 3 karakter olmalıdır.")]
    [MaxLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "İçerik zorunludur.")]
    [MinLength(10, ErrorMessage = "İçerik en az 10 karakter olmalıdır.")]
    [MaxLength(5000, ErrorMessage = "İçerik en fazla 5000 karakter olabilir.")]
    public string Content { get; set; } = string.Empty;
}


