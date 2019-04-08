using DataContext;

namespace DataContextDataFiller.Interfaces
{
    public interface IDataInserter
    {
        void InsertValues(ApplicationContext dbContext);
    }
}