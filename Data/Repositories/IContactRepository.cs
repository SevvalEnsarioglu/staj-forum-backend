using staj_forum_backend.Models;

namespace staj_forum_backend.Data.Repositories;

public interface IContactRepository
{
    Task<ContactMessage?> GetContactMessageByIdAsync(int id);
    Task<IEnumerable<ContactMessage>> GetContactMessagesAsync(int page, int pageSize, bool? isRead);
    Task<int> GetContactMessagesCountAsync(bool? isRead);
    Task<ContactMessage> CreateContactMessageAsync(ContactMessage message);
    Task<ContactMessage> MarkAsReadAsync(int id);
    Task<bool> DeleteContactMessageAsync(int id);
}


