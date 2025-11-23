using staj_forum_backend.DTOs.Common;
using staj_forum_backend.DTOs.Contact;

namespace staj_forum_backend.Services;

public interface IContactService
{
    Task<ContactMessageDto> CreateContactMessageAsync(CreateContactDto createContactDto);
    Task<PagedResultDto<ContactMessageDto>> GetContactMessagesAsync(int page, int pageSize, bool? isRead);
    Task<ContactMessageDto?> GetContactMessageByIdAsync(int id);
    Task<ContactMessageDto> MarkAsReadAsync(int id);
    Task<bool> DeleteContactMessageAsync(int id);
}


