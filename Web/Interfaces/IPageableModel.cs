using System.Collections.Generic;

namespace Web.Interfaces
{
    public interface IPageableModel<T> : IPageableModel
    {
        IList<T> Items { get; set; }
    }

    public interface IPageableModel
    {
        int Page { get; set; }
        int TotalPages { get; set; }
    }
}