using AutoMapper;
using Inventory.API.Entities;
using Inventory.API.Models;

namespace Inventory.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap();
        }
    }
}