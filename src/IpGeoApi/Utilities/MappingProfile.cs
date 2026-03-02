using AutoMapper;
using IpGeoApi.DTOs;
using IpGeoApi.Models;

namespace IpGeoApi.Utilities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<History, HistoryDto>().ReverseMap();
    }
}
