using AutoMapper;
using JacksonVeroneze.StockService.Application.DTO.Adjustment;
using JacksonVeroneze.StockService.Application.DTO.AdjustmentItem;
using JacksonVeroneze.StockService.Application.DTO.Output;
using JacksonVeroneze.StockService.Application.DTO.OutputItem;
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

            CreateMap<AddOrUpdateAdjustmentDto, Adjustment>()
                .ConstructUsing(x => new Adjustment(x.Description, x.Date));

            CreateMap<AddOrUpdateOutputDto, Output>()
                .ConstructUsing(x => new Output(x.Description, x.Date));

            CreateMap<AddOrUpdatePurchaseDto, Purchase>()
                .ConstructUsing(x => new Purchase(x.Description, x.Date));

            CreateMap<Product, ProductDto>();
            CreateMap<Adjustment, AdjustmentDto>();
            CreateMap<Output, OutputDto>();
            CreateMap<Purchase, PurchaseDto>();

            CreateMap<AdjustmentItem, AdjustmentItemDto>();
            CreateMap<OutputItem, OutputItemDto>();
            CreateMap<PurchaseItem, PurchaseItemDto>()
                .ForMember(x => x.PurchaseId, f => f.MapFrom(x => x.Purchase.Id))
                .ForMember(x => x.ProductId, f => f.MapFrom(x => x.Product.Id));
        }
    }
}
