using System.ComponentModel.DataAnnotations;

namespace staj_forum_backend.DTOs.Contact;

public class CreateContactDto
{
    [Required(ErrorMessage = "Ad-Soyad zorunludur.")]
    [MinLength(2, ErrorMessage = "Ad-Soyad en az 2 karakter olmalıdır.")]
    [MaxLength(100, ErrorMessage = "Ad-Soyad en fazla 100 karakter olabilir.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    [MaxLength(255, ErrorMessage = "E-posta en fazla 255 karakter olabilir.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Konu zorunludur.")]
    [MinLength(3, ErrorMessage = "Konu en az 3 karakter olmalıdır.")]
    [MaxLength(200, ErrorMessage = "Konu en fazla 200 karakter olabilir.")]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mesaj zorunludur.")]
    [MinLength(10, ErrorMessage = "Mesaj en az 10 karakter olmalıdır.")]
    [MaxLength(2000, ErrorMessage = "Mesaj en fazla 2000 karakter olabilir.")]
    public string Message { get; set; } = string.Empty;
}


