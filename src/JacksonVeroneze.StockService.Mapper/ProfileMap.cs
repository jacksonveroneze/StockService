using AutoMapper;
using JacksonVeroneze.StockService.Application.DTO.Product;
using JacksonVeroneze.StockService.Application.DTO.Purchase;
using JacksonVeroneze.StockService.Application.DTO.PurchaseItem;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Mapper
{
    public class ProfileMapStock : Profile
    {
        public ProfileMapStock()
        {
            CreateMap<AddOrUpdateProductDto, Product>()
                .ConstructUsing(x => new Product(x.Description));

            CreateMap<Product, ProductDto>();

            CreateMap<AddOrUpdatePurchaseDto, Purchase>()
                .ConstructUsing(x => new Purchase(x.Description, x.Date));

            CreateMap<Purchase, PurchaseDto>();

            CreateMap<PurchaseItem, PurchaseItemDto>();
        }
    }
}
