using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.DTO.Product;
using JacksonVeroneze.StockService.Application.DTO.Product.Validations;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Application.Util;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;

namespace JacksonVeroneze.StockService.Application.Services
{
    public class ProductApplicationService : IProductApplicationService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public ProductApplicationService(IMapper mapper, IProductRepository productRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<ProductResultDto> FindAsync(Guid id)
            => _mapper.Map<ProductResultDto>(await _productRepository.FindAsync(id));

        public async Task<IEnumerable<ProductResultDto>> FindAllAsync()
            => _mapper.Map<IEnumerable<ProductResultDto>>(await _productRepository.FindAllAsync());

        public async Task<ApplicationDataResult<ProductResultDto>> AddASync(ProductRequestDto productRequestDto)
        {
            ValidationResult validationResult = await new ProductDtoValidator()
                .ValidateAsync(productRequestDto);

            if (validationResult.IsValid is false)
                return new ApplicationDataResult<ProductResultDto>(validationResult.Errors.Select(x => x.ErrorMessage));

            Product product = _mapper.Map<Product>(productRequestDto);

            await _productRepository.AddAsync(product);

            await _productRepository.UnitOfWork.CommitAsync();

            return new ApplicationDataResult<ProductResultDto>(_mapper.Map<ProductResultDto>(product));
        }

        public Task<ApplicationDataResult<ProductResultDto>> UpdateASync(ProductRequestDto productRequestDto) => throw new NotImplementedException();

        public async Task RemoveASync(Guid id)
        {
            Product product = await _productRepository.FindAsync(id);

            _productRepository.Remove(product);

            await _productRepository.UnitOfWork.CommitAsync();
        }
    }
}
