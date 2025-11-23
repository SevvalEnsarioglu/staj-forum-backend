using AutoMapper;
using staj_forum_backend.DTOs.Contact;
using staj_forum_backend.DTOs.Forum;
using staj_forum_backend.Models;

namespace staj_forum_backend.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Forum mappings
        CreateMap<Topic, TopicDto>()
            .ForMember(dest => dest.ReplyCount, opt => opt.MapFrom(src => src.Replies.Count));

        CreateMap<Topic, TopicDetailDto>()
            .ForMember(dest => dest.ReplyCount, opt => opt.MapFrom(src => src.Replies.Count))
            .ForMember(dest => dest.Replies, opt => opt.MapFrom(src => src.Replies));

        CreateMap<CreateTopicDto, Topic>();

        CreateMap<UpdateTopicDto, Topic>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.AuthorName, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ViewCount, opt => opt.Ignore())
            .ForMember(dest => dest.Replies, opt => opt.Ignore());

        CreateMap<Reply, ReplyDto>();

        CreateMap<CreateReplyDto, Reply>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.TopicId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Topic, opt => opt.Ignore());

        // Contact mappings
        CreateMap<ContactMessage, ContactMessageDto>();

        CreateMap<CreateContactDto, ContactMessage>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsRead, opt => opt.Ignore());
    }
}


