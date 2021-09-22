using AutoMapper;
using EventApp.Data.Dtos;
using EventApp.Models.Models;
using System.Collections.Generic;

namespace EventApp.EntityMapper
{
    public static class EntityMapper
    {
        private static readonly MapperConfiguration _config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<EventDto, Event>();
            cfg.CreateMap<RegistrationDto, Registration>();
        });

        private static readonly IMapper s_mapper = _config.CreateMapper();

        internal static List<Event> MapEventsDtos(List<EventDto> dtos)
        {
            return s_mapper.Map<List<Event>>(dtos);
        }

        internal static List<Registration> MapRegistrationsDtos(List<RegistrationDto> dtos)
        {
            return s_mapper.Map<List<Registration>>(dtos);
        }
    }
}
