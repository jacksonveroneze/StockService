using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.DTO.Purchase;
using JacksonVeroneze.StockService.Application.DTO.PurchaseItem;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Application.Util;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;

namespace JacksonVeroneze.StockService.Application.Services
{
    public class PurchaseApplicationService : IPurchaseApplicationService
    {
        private readonly IMapper _mapper;
        private readonly IPurchaseService _purchaseService;
        private readonly IPurchaseRepository _purchaseRepository;

        public PurchaseApplicationService(IMapper mapper, IPurchaseService purchaseService,
            IPurchaseRepository purchaseRepository)
        {
            _mapper = mapper;
            _purchaseService = purchaseService;
            _purchaseRepository = purchaseRepository;
        }

        public async Task<PurchaseDto> FindAsync(Guid id)
            => _mapper.Map<PurchaseDto>(
                await _purchaseRepository.FindAsync(id));

        public async Task<IEnumerable<PurchaseDto>> FindAllAsync()
            => _mapper.Map<IEnumerable<PurchaseDto>>(
                await _purchaseRepository.FindAllAsync());

        public async Task<ApplicationDataResult<PurchaseDto>> AddAsync(AddOrUpdatePurchaseDto data)
        {
            ValidationResult validationResult = await data.Validate();

            if (validationResult.IsValid is false)
                return new ApplicationDataResult<PurchaseDto>(
                    validationResult.Errors.Select(x => x.ErrorMessage));

            Purchase purchase = _mapper.Map<Purchase>(data);

            await _purchaseRepository.AddAsync(purchase);

            await _purchaseRepository.UnitOfWork.CommitAsync();

            return new ApplicationDataResult<PurchaseDto>(
                _mapper.Map<PurchaseDto>(purchase));
        }

        public async Task<ApplicationDataResult<PurchaseDto>> UpdateAsync(AddOrUpdatePurchaseDto data)
        {
            ValidationResult validationResult = await data.Validate();

            if (validationResult.IsValid is false)
                return new ApplicationDataResult<PurchaseDto>(
                    validationResult.Errors.Select(x => x.ErrorMessage));

            Purchase purchase = _mapper.Map<Purchase>(data);

            _purchaseRepository.Update(purchase);

            await _purchaseRepository.UnitOfWork.CommitAsync();

            return new ApplicationDataResult<PurchaseDto>(
                _mapper.Map<PurchaseDto>(purchase));
        }

        public async Task RemoveAsync(Guid id)
        {
            Purchase purchase = await _purchaseRepository.FindAsync(id);

            _purchaseRepository.Remove(purchase);

            await _purchaseRepository.UnitOfWork.CommitAsync();
        }

        public async Task Close(Guid id)
        {
            Purchase purchase = await _purchaseRepository.FindAsync(id);

            await _purchaseService.Close(purchase);
        }

        public async Task<PurchaseItemDto> FindItemAsync(Guid id, Guid itemId)
        {
            Purchase purchase = await _purchaseRepository.FindAsync(id);

            return _mapper.Map<PurchaseItemDto>(purchase.FindItemById(id));
        }

        public async Task<IEnumerable<PurchaseItemDto>> FindItensAsync(Guid id)
        {
            Purchase purchase = await _purchaseRepository.FindAsync(id);

            return _mapper.Map<IEnumerable<PurchaseItemDto>>(purchase.Items);
        }

        public async Task<ApplicationDataResult<PurchaseItemDto>> AddItemAsync(Guid id, AddOrUpdatePurchaseItemDto data)
        {
            ValidationResult validationResult = await data.Validate();

            if (validationResult.IsValid is false)
                return new ApplicationDataResult<PurchaseItemDto>(
                    validationResult.Errors.Select(x => x.ErrorMessage));

            Purchase purchase = await _purchaseRepository.FindAsync(id);

            PurchaseItem purchaseItem = _mapper.Map<PurchaseItem>(data);

            await _purchaseService.AddItem(purchase, purchaseItem);

            return new ApplicationDataResult<PurchaseItemDto>(
                _mapper.Map<PurchaseItemDto>(purchaseItem));
        }

        public async Task<ApplicationDataResult<PurchaseItemDto>> UpdateItemAsync(Guid id, AddOrUpdatePurchaseItemDto data)
        {
            ValidationResult validationResult = await data.Validate();

            if (validationResult.IsValid is false)
                return new ApplicationDataResult<PurchaseItemDto>(
                    validationResult.Errors.Select(x => x.ErrorMessage));

            Purchase purchase = await _purchaseRepository.FindAsync(id);

            PurchaseItem purchaseItem = _mapper.Map<PurchaseItem>(data);

            await _purchaseService.UpdateItem(purchase, purchaseItem);

            return new ApplicationDataResult<PurchaseItemDto>(
                _mapper.Map<PurchaseItemDto>(purchaseItem));
        }

        public async Task RemoveItemAsync(Guid id, Guid itemId)
        {
            Purchase purchase = await _purchaseRepository.FindAsync(id);

            PurchaseItem purchaseItem = purchase.FindItemById(id);

            await _purchaseService.RemoveItem(purchase, purchaseItem);
        }
    }
}
