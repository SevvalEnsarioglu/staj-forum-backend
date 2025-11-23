using Microsoft.EntityFrameworkCore;
using staj_forum_backend.Models;

namespace staj_forum_backend.Data.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly ApplicationDbContext _context;

    public ContactRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ContactMessage?> GetContactMessageByIdAsync(int id)
    {
        return await _context.ContactMessages.FindAsync(id);
    }

    public async Task<IEnumerable<ContactMessage>> GetContactMessagesAsync(int page, int pageSize, bool? isRead)
    {
        var query = _context.ContactMessages.AsQueryable();

        // Filter by read status
        if (isRead.HasValue)
        {
            query = query.Where(m => m.IsRead == isRead.Value);
        }

        // Order by creation date (newest first)
        query = query.OrderByDescending(m => m.CreatedAt);

        // Pagination
        return await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetContactMessagesCountAsync(bool? isRead)
    {
        var query = _context.ContactMessages.AsQueryable();

        if (isRead.HasValue)
        {
            query = query.Where(m => m.IsRead == isRead.Value);
        }

        return await query.CountAsync();
    }

    public async Task<ContactMessage> CreateContactMessageAsync(ContactMessage message)
    {
        message.CreatedAt = DateTime.UtcNow;
        message.IsRead = false;
        _context.ContactMessages.Add(message);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<ContactMessage> MarkAsReadAsync(int id)
    {
        var message = await GetContactMessageByIdAsync(id);
        if (message == null)
            throw new KeyNotFoundException($"Contact message with id {id} not found");

        message.IsRead = true;
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<bool> DeleteContactMessageAsync(int id)
    {
        var message = await GetContactMessageByIdAsync(id);
        if (message == null)
            return false;

        _context.ContactMessages.Remove(message);
        await _context.SaveChangesAsync();
        return true;
    }
}


