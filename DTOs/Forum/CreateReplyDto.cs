using System.ComponentModel.DataAnnotations;

namespace staj_forum_backend.DTOs.Forum;

public class CreateReplyDto
{
    [Required(ErrorMessage = "İçerik zorunludur.")]
    [MinLength(5, ErrorMessage = "Yanıt en az 5 karakter olmalıdır.")]
    [MaxLength(2000, ErrorMessage = "Yanıt en fazla 2000 karakter olabilir.")]
    public string Content { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ad-Soyad zorunludur.")]
    [MinLength(2, ErrorMessage = "Ad-Soyad en az 2 karakter olmalıdır.")]
    [MaxLength(100, ErrorMessage = "Ad-Soyad en fazla 100 karakter olabilir.")]
    public string AuthorName { get; set; } = string.Empty;
}


