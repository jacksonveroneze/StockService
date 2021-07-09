using System.Collections.Generic;
using JacksonVeroneze.StockService.Core.Notifications;

namespace JacksonVeroneze.StockService.Core.ValidationPattern
{
    public interface IValidator<TModel>
    {
        IValidator<TModel> AddRule(IValidationRule<TModel> rule);

        IList<Notification> Validate(TModel model);
    }
}
