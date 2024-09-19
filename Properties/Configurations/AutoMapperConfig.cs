using AutoMapper;
using CollegeApp.Models;

namespace CollegeApp.Properties.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {

            CreateMap<Student, StudentDTO>()
                .ForMember(dest => dest.NameDTO, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.EmailDTO, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.AddressDTO, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.AdmissionDateDTO, opt => opt.MapFrom(src => src.AdmissionDate))
                .ReverseMap();

        }
    }
}
