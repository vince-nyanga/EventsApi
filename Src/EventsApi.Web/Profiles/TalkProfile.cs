using AutoMapper;
using EventsApi.Core.Entities;
using EventsApi.Web.Models.Talks;

namespace EventsApi.Web.Profiles
{
    public class TalkProfile : Profile
    {
        public TalkProfile()
        {
            CreateMap<Talk, TalkDto>();
            CreateMap<TalkForUpdateDto, Talk>();
            CreateMap<Talk, TalkForUpdateDto>();
        }
    }
}
