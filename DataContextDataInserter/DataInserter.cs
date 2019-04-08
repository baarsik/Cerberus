using DataContext;
using DataContext.Models;
using DataContextDataFiller.Data;
using DataContextDataFiller.Interfaces;

namespace DataContextDataFiller
{
    public class DataInserter : IDataInserter
    {
        private readonly IData<Language> _languageData = new LanguageData();
        private readonly IData<WebNovelTag> _webNovelTagData = new WebNovelTagData();
        
        public void InsertValues(ApplicationContext dbContext)
        {
            _languageData.InsertInto(dbContext.Languages);
            _webNovelTagData.InsertInto(dbContext.WebNovelTag);
        }
    }
}