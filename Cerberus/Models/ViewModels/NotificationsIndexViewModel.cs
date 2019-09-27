using System;
using System.Collections.Generic;
using Cerberus.Interfaces;
using DataContext.Models;

namespace Cerberus.Models.ViewModels
{
    public class NotificationsIndexViewModel : IPageableViewModel<NotificationsIndexViewModelItem>
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