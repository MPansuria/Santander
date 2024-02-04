using AutoMapper;
using HackerNewsClientAPI.Models;

namespace HackerNewsClientAPI
{
    public class AutoMapperConfiguration
    {
        public static MapperConfiguration GetConfig()
        {
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Story, StoryDto>()
                .ForMember(dest => dest.postedBy, opt => opt.MapFrom(src => src.by))
                .ForMember(dest => dest.uri, opt => opt.MapFrom(src => src.url))
                .ForMember(dest => dest.commentCount, opt => opt.MapFrom(src => src.descendants))
                );                              

            return config;
        }
        public static IMapper GetMapper()
        {
            return GetConfig().CreateMapper();
        }
    }
}
