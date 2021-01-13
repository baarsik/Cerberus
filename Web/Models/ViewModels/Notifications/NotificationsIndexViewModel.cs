using System;
using System.Collections.Generic;
using Web.Interfaces;

namespace Web.Models.ViewModels.Notifications
{
    public class NotificationsIndexViewModel : IPageableModel<NotificationsIndexViewModelItem>
    {
        public IList<NotificationsIndexViewModelItem> Items { get; set; }
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