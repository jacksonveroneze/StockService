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
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ConstructUsing(x => new Product(x.Description));

            CreateMap<AddOrUpdateAdjustmentDto, Adjustment>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ConstructUsing(x => new Adjustment(x.Description, x.Date));

            CreateMap<AddOrUpdateOutputDto, Output>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ConstructUsing(x => new Output(x.Description, x.Date));

            CreateMap<AddOrUpdatePurchaseDto, Purchase>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ConstructUsing(x => new Purchase(x.Description, x.Date));

            CreateMap<Product, ProductDto>();
            CreateMap<Adjustment, AdjustmentDto>();
            CreateMap<Output, OutputDto>();
            CreateMap<Purchase, PurchaseDto>();

            CreateMap<AdjustmentItem, AdjustmentItemDto>()
                .ForMember(x => x.AdjustmentId, f => f.MapFrom(x => x.Adjustment.Id))
                .ForMember(x => x.ProductId, f => f.MapFrom(x => x.Product.Id));

            CreateMap<OutputItem, OutputItemDto>()
                .ForMember(x => x.OutputId, f => f.MapFrom(x => x.Output.Id))
                .ForMember(x => x.ProductId, f => f.MapFrom(x => x.Product.Id));

            CreateMap<PurchaseItem, PurchaseItemDto>()
                .ForMember(x => x.PurchaseId, f => f.MapFrom(x => x.Purchase.Id))
                .ForMember(x => x.ProductId, f => f.MapFrom(x => x.Product.Id));
        }
    }
}
