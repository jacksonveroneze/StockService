using JacksonVeroneze.StockService.Core.Notifications;

namespace JacksonVeroneze.StockService.Core.ValidationPattern
{
    public interface IValidationRule<T>
    {
        Notification Error { get; }

        bool Validate(T value);

        bool StopValidation { get; set; }
    }
}
