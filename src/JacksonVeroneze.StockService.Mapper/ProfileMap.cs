using AutoMapper;
using JacksonVeroneze.StockService.Application.DTO.Adjustment;
using JacksonVeroneze.StockService.Application.DTO.AdjustmentItem;
using JacksonVeroneze.StockService.Application.DTO.Devolution;
using JacksonVeroneze.StockService.Application.DTO.DevolutionItem;
using JacksonVeroneze.StockService.Application.DTO.Output;
using JacksonVeroneze.StockService.Application.DTO.OutputItem;
using JacksonVeroneze.StockService.Application.DTO.Product;
using JacksonVeroneze.StockService.Application.DTO.Purchase;
using JacksonVeroneze.StockService.Application.DTO.PurchaseItem;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Events.Product;
using JacksonVeroneze.StockService.Domain.Models;

namespace JacksonVeroneze.StockService.Mapper
{
    public class ProfileMapStock : Profile
    {
        public ProfileMapStock()
        {
            MapDtoToEntity();
            MapEntityToDto();
            MapPageableEntityToPageableDto();
            MapEntityToEvent();
        }

        private void MapDtoToEntity()
        {
            CreateMap<AddOrUpdateProductDto, Product>()
                .ConstructUsing(x => new Product(x.Description))
                .ForMember(x => x.TenantId, b => b.Ignore());

            CreateMap<AddOrUpdateAdjustmentDto, Adjustment>()
                .ConstructUsing(x => new Adjustment(x.Description, x.Date))
                .ForMember(x => x.TenantId, b => b.Ignore());

            CreateMap<AddOrUpdateOutputDto, Output>()
                .ConstructUsing(x => new Output(x.Description, x.Date))
                .ForMember(x => x.TenantId, b => b.Ignore());

            CreateMap<AddOrUpdatePurchaseDto, Purchase>()
                .ConstructUsing(x => new Purchase(x.Description, x.Date))
                .ForMember(x => x.TenantId, b => b.Ignore());
        }

        private void MapEntityToDto()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<Adjustment, AdjustmentDto>();
            CreateMap<Output, OutputDto>();
            CreateMap<Purchase, PurchaseDto>();
            CreateMap<Devolution, DevolutionDto>();

            CreateMap<AdjustmentItemModel, AdjustmentItemDto>();
            CreateMap<OutputItemModel, OutputItemDto>();
            CreateMap<PurchaseItemModel, PurchaseItemDto>();
            CreateMap<DevolutionItemModel, DevolutionItemDto>();

            CreateMap<AdjustmentItem, AdjustmentItemDto>()
                .ForMember(x => x.AdjustmentId, f => f.MapFrom(x => x.Adjustment.Id))
                .ForMember(x => x.ProductId, f => f.MapFrom(x => x.Product.Id))
                .ForMember(x => x.ProductDescription, f => f.Ignore());

            CreateMap<OutputItem, OutputItemDto>()
                .ForMember(x => x.OutputId, f => f.MapFrom(x => x.Output.Id))
                .ForMember(x => x.ProductId, f => f.MapFrom(x => x.Product.Id))
                .ForMember(x => x.ProductDescription, f => f.Ignore());

            CreateMap<PurchaseItem, PurchaseItemDto>()
                .ForMember(x => x.PurchaseId, f => f.MapFrom(x => x.Purchase.Id))
                .ForMember(x => x.ProductId, f => f.MapFrom(x => x.Product.Id))
                .ForMember(x => x.ProductDescription, f => f.Ignore());

            CreateMap<DevolutionItem, DevolutionItemDto>()
                .ForMember(x => x.DevolutionId, f => f.MapFrom(x => x.Devolution.Id))
                .ForMember(x => x.ProductId, f => f.MapFrom(x => x.Product.Id))
                .ForMember(x => x.ProductDescription, f => f.Ignore());
        }

        private void MapEntityToEvent()
        {
            CreateMap<Product, ProductAddedEvent>()
                .ConstructUsing(x => new ProductAddedEvent(x.Id))
                .ForMember(x => x.Description, b => b.MapFrom(x => x.Description))
                .ForMember(x => x.IsActive, b => b.MapFrom(x => x.IsActive));
        }

        private void MapPageableEntityToPageableDto()
        {
            CreateMap<Pageable<Product>, Pageable<ProductDto>>();
            CreateMap<Pageable<Adjustment>, Pageable<AdjustmentDto>>();
            CreateMap<Pageable<Output>, Pageable<OutputDto>>();
            CreateMap<Pageable<Purchase>, Pageable<PurchaseDto>>();
            CreateMap<Pageable<Devolution>, Pageable<DevolutionDto>>();
        }
    }
}
