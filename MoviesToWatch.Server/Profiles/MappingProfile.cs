using AutoMapper;
using MoviesToWatch.Server.Models;

namespace MoviesToWatch.Server.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DBApiMovie, Movie>();
        }
    }
}
