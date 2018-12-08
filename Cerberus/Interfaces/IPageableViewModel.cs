using System.Collections.Generic;

namespace Cerberus.Interfaces
{
    public interface IPageableViewModel<T>
    {
        IEnumerable<T> Items { get; set; }
        int Page { get; set; }
        int TotalPages { get; set; }
    }
}