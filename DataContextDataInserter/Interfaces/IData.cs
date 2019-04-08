using System.Collections.Generic;
using DataContext.Models;
using Microsoft.EntityFrameworkCore;

namespace DataContextDataFiller.Interfaces
{
    public interface IData<T> where T : BaseEntity
    {
        ICollection<T> GetDataCollection();
        void InsertInto(DbSet<T> dbSet);
    }
}