using System.Collections.Generic;
using System.Linq;
using JacksonVeroneze.StockService.Core;

namespace JacksonVeroneze.StockService.Application.Util
{
    public class ApplicationDataResult<T>
    {
        public IEnumerable<string> Errors { get; } = new List<string>();

        public T Data { get; }

        public bool IsSuccess => !Errors.Any();

        private ApplicationDataResult(IEnumerable<string> errors) => Errors = errors;

        private ApplicationDataResult(T data) => Data = data;

        public ApplicationDataResult()
        {
        }

        public static ApplicationDataResult<T> FactoryFromNotificationContext(NotificationContext notificationContext)
            => new(notificationContext.Notifications.Select(x => x.Message));

        public static ApplicationDataResult<T> FactoryFromData(T data)
            => new(data);

        public static ApplicationDataResult<T> FactoryFromEmpty()
            => new();
    }
}
