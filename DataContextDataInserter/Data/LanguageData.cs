using System;
using System.Collections.Generic;
using System.Linq;
using DataContext.Models;
using DataContextDataFiller.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataContextDataFiller.Data
{
    public class LanguageData : IData<Language>
    {
        private readonly ICollection<Language> _data = new List<Language>
        {
            new Language {Id = Guid.Parse("602B9147-CD29-4995-C6C3-08D6A2072C66"), Code = "ru", CountryFlagIconName = "ru", GlobalName = "Russian", LocalName = "Русский"},
            new Language {Id = Guid.Parse("CA693E56-5D49-42BF-B688-08D6ABD97C45"), Code = "en", CountryFlagIconName = "us", GlobalName = "English", LocalName = "English"}
        };
        
        public ICollection<Language> GetDataCollection()
        {
            return _data.ToList();
        }

        public void InsertInto(DbSet<Language> dbSet)
        {
            var missingData = _data
                .Where(c => dbSet.All(d => d.Id != c.Id))
                .ToList();
            
            dbSet.AddRange(missingData);
        }
    }
}