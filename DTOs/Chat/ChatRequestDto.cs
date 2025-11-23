using System.ComponentModel.DataAnnotations;

namespace staj_forum_backend.DTOs.Chat;

public class ChatRequestDto
{
    [Required(ErrorMessage = "Mesaj zorunludur.")]
    [MinLength(1, ErrorMessage = "Mesaj boş olamaz.")]
    [MaxLength(2000, ErrorMessage = "Mesaj en fazla 2000 karakter olabilir.")]
    public string Message { get; set; } = string.Empty;

    public string? ConversationId { get; set; }
}


