using JacksonVeroneze.StockService.Core.Notifications;

namespace JacksonVeroneze.StockService.Application.Validations
{
    /// <summary>
    /// Class responsible for validator.
    /// </summary>
    public class Validator
    {
        protected Notification CreateNotification(string key, string message) => new(key, message);
    }
}
