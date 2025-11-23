using AutoMapper;
using staj_forum_backend.Data.Repositories;
using staj_forum_backend.DTOs.Common;
using staj_forum_backend.DTOs.Contact;

namespace staj_forum_backend.Services;

public class ContactService : IContactService
{
    private readonly IContactRepository _contactRepository;
    private readonly IMapper _mapper;

    public ContactService(IContactRepository contactRepository, IMapper mapper)
    {
        _contactRepository = contactRepository;
        _mapper = mapper;
    }

    public async Task<ContactMessageDto> CreateContactMessageAsync(CreateContactDto createContactDto)
    {
        var contactMessage = _mapper.Map<Models.ContactMessage>(createContactDto);
        var createdMessage = await _contactRepository.CreateContactMessageAsync(contactMessage);
        return _mapper.Map<ContactMessageDto>(createdMessage);
    }

    public async Task<PagedResultDto<ContactMessageDto>> GetContactMessagesAsync(int page, int pageSize, bool? isRead)
    {
        // Validate pagination parameters
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var messages = await _contactRepository.GetContactMessagesAsync(page, pageSize, isRead);
        var totalCount = await _contactRepository.GetContactMessagesCountAsync(isRead);

        var messageDtos = _mapper.Map<List<ContactMessageDto>>(messages);

        return new PagedResultDto<ContactMessageDto>
        {
            Data = messageDtos,
            Pagination = new PaginationDto
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            }
        };
    }

    public async Task<ContactMessageDto?> GetContactMessageByIdAsync(int id)
    {
        var message = await _contactRepository.GetContactMessageByIdAsync(id);
        if (message == null)
            return null;

        return _mapper.Map<ContactMessageDto>(message);
    }

    public async Task<ContactMessageDto> MarkAsReadAsync(int id)
    {
        var message = await _contactRepository.MarkAsReadAsync(id);
        return _mapper.Map<ContactMessageDto>(message);
    }

    public async Task<bool> DeleteContactMessageAsync(int id)
    {
        return await _contactRepository.DeleteContactMessageAsync(id);
    }
}


