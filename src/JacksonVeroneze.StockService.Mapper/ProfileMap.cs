using AutoMapper;
using JacksonVeroneze.StockService.Application.DTO;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Mapper
{
    public class ProfileMapStock : Profile
    {
        public ProfileMapStock()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
        }
    }
}
