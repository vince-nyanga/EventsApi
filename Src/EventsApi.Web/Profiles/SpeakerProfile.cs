using AutoMapper;
using EventsApi.Core.Entities;
using EventsApi.Web.Models.Speaker;

namespace EventsApi.Web.Profiles
{
    public class SpeakerProfile : Profile
    {
        public SpeakerProfile()
        {
            CreateMap<Speaker, SpeakerDto>();
            CreateMap<SpeakerForUpdateDto, Speaker>();
            CreateMap<Speaker, SpeakerForUpdateDto>();
        }
    }
}
