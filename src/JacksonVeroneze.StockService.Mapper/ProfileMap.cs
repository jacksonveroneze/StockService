using AutoMapper;
using JacksonVeroneze.StockService.Application.DTO;
using JacksonVeroneze.StockService.Application.DTO.Product;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Mapper
{
    public class ProfileMapStock : Profile
    {
        public ProfileMapStock()
        {
            CreateMap<ProductRequestDto, Product>()
                .ConstructUsing(x => new Product(x.Description))
                .ReverseMap();
        }
    }
}
