using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace JacksonVeroneze.StockService.Core.Notifications
{
    public class NotificationContext
    {
        private readonly List<Notification> _notifications;

        public IReadOnlyCollection<Notification> Notifications => _notifications;

        public bool HasNotifications => _notifications.Any();

        public NotificationContext()
            => _notifications = new List<Notification>();

        private NotificationContext Add(Notification notification)
        {
            _notifications.Add(notification);

            return this;
        }

        public NotificationContext AddNotification(string key, string message)
            => Add(new Notification(key, message));

        public NotificationContext AddNotification(Notification notification)
            => Add(notification);

        public NotificationContext AddNotifications(ValidationResult validationResult)
        {
            foreach (ValidationFailure error in validationResult.Errors)
                AddNotification(error.PropertyName, error.ErrorMessage.Replace("'", String.Empty));

            return this;
        }
    }
}
