using System;
using System.Collections.Generic;
using Cerberus.Interfaces;

namespace Cerberus.Models.ViewModels.Notifications
{
    public class NotificationsIndexViewModel : IPageableModel<NotificationsIndexViewModelItem>
    {
        public IEnumerable<NotificationsIndexViewModelItem> Items { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }

    public class NotificationsIndexViewModelItem
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public DateTime NotificationDate { get; set; }
        public bool IsRead { get; set; }
    }
}