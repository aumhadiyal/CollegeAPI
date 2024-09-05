using AutoMapper;
using CollegeApp.Models;

namespace CollegeApp.Properties.Configurations
{
    public class AutoMapperConfig:Profile
    {
        public AutoMapperConfig()
        {

            CreateMap<Student, StudentDTO>().ReverseMap();
        }
    }
}
