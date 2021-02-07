using System.Collections.Generic;
using System.Linq;

namespace JacksonVeroneze.StockService.Application.Util
{
    public class ApplicationDataResult<T>
    {
        public IEnumerable<string> Errors { get; private set; } = new List<string>();

        public T Data { get; private set; }

        public bool IsSuccess => !Errors.Any();

        public ApplicationDataResult(IEnumerable<string> errors) => Errors = errors;

        public ApplicationDataResult(T data) => Data = data;
    }
}
