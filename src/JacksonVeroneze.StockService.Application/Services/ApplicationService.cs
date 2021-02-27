using System.Linq;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.Util;

namespace JacksonVeroneze.StockService.Application.Services
{
    public class ApplicationService
    {
        public ApplicationDataResult<T> FactoryResultFromData<T>(T data)
            => new(data);


        public ApplicationDataResult<T> FactoryFromValidationResult<T>(ValidationResult validationResult)
            => new(validationResult.Errors.Select(x => x.ErrorMessage));
    }
}
