using System.Collections.Generic;
using System.Linq;
using JacksonVeroneze.StockService.Core.Notifications;

namespace JacksonVeroneze.StockService.Application.Util
{
    public class ApplicationDataResult<T>
    {
        public List<Notification> Errors { get; }

        public T Data { get; }

        public readonly bool IsSuccess = true;

        private ApplicationDataResult(List<Notification> errors)
        {
            Errors = errors;
            IsSuccess = false;
        }

        private ApplicationDataResult(T data) => Data = data;

        private ApplicationDataResult()
        {
        }

        public static ApplicationDataResult<T> FactoryFromNotificationContext(NotificationContext notificationContext)
            => new(notificationContext.Notifications.ToList());

        public static ApplicationDataResult<T> FactoryFromData(T data)
            => new(data);

        public static ApplicationDataResult<T> FactoryFromEmpty()
            => new();
    }
}
