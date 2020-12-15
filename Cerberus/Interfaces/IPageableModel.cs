using System.Collections.Generic;

namespace Cerberus.Interfaces
{
    public interface IPageableModel<T> : IPageableModel
    {
        IEnumerable<T> Items { get; set; }
    }

    public interface IPageableModel
    {
        int Page { get; set; }
        int TotalPages { get; set; }
    }
}