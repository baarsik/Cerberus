using System;
using System.Collections.Generic;
using Cerberus.Interfaces;
using DataContext.Models;

namespace Cerberus.Models.ViewModels.Notifications
{
    public class ManageNotificationsViewModel : IPageableViewModel<ManageNotificationsViewModelItem>
    {
        public IEnumerable<ManageNotificationsViewModelItem> Items { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }
    
    public class ManageNotificationsViewModelItem
    {
        public Guid Id { get; set; }
        public WebNovelContent WebNovelContent { get; set; }
        public string WebNovelURL { get; set; }
    }
}