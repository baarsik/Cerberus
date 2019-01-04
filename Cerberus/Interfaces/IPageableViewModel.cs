using System.Collections.Generic;

namespace Cerberus.Interfaces
{
    public interface IPageableViewModel<T> : IPageableViewModel
    {
        IEnumerable<T> Items { get; set; }
    }

    public interface IPageableViewModel
    {
        int Page { get; set; }
        int TotalPages { get; set; }
    }
}