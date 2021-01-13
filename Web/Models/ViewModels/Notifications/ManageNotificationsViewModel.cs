using System;
using System.Collections.Generic;
using Web.Interfaces;
using DataContext.Models;

namespace Web.Models.ViewModels.Notifications
{
    public class ManageNotificationsViewModel : IPageableModel<ManageNotificationsViewModelItem>
    {
        public IList<ManageNotificationsViewModelItem> Items { get; set; }
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