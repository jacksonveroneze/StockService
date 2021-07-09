using System.Collections.Generic;
using JacksonVeroneze.StockService.Core.Notifications;

namespace JacksonVeroneze.StockService.Core.ValidationPattern
{
    public class Validator<TModel> : IValidator<TModel>
    {
        private readonly List<IValidationRule<TModel>> _validators = new();

        public IValidator<TModel> AddRule(IValidationRule<TModel> rule)
        {
            _validators.Add(rule);
            return this;
        }

        public IList<Notification> Validate(TModel model)
        {
            IList<Notification> resultValidation = new List<Notification>();

            foreach (IValidationRule<TModel> validationRule in _validators)
            {
                bool result = validationRule.Validate(model);

                if (result is false)
                    resultValidation.Add(new Notification(validationRule.Error.Key, validationRule.Error.Message));

                if (result is false && validationRule.StopValidation)
                    break;
            }

            return resultValidation;
        }
    }
}
