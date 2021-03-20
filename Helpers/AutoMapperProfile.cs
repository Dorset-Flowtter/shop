using AutoMapper;
using shop.DTO;
using shop.Models;

namespace shop.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Customer, UserModel>();
            CreateMap<RegisterModel, Customer>();
            CreateMap<UpdateModel, Customer>();
        }
    }
}