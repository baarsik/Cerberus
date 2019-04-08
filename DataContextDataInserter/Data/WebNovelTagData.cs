using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataContext.Models;
using DataContextDataFiller.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DataContextDataFiller.Data
{
    public class WebNovelTagData : IData<WebNovelTag>
    {
        private ICollection<WebNovelTag> _data = new List<WebNovelTag>();
        
        public ICollection<WebNovelTag> GetDataCollection()
        {
            if (!_data.Any())
            {
                LoadData();
            }
            
            return _data.ToList();
        }

        public void InsertInto(DbSet<WebNovelTag> dbSet)
        {
            if (!_data.Any())
            {
                LoadData();
            }
            
            var missingData = _data
                .Where(c => dbSet.All(d => d.FallbackName != c.FallbackName))
                .ToList();
            
            dbSet.AddRange(missingData);
        }

        private void LoadData()
        {
            var fileContents = File.ReadAllText("RawData/web_novel_tags.min.json");
            var tagTitleList = JsonConvert.DeserializeObject<List<string>>(fileContents);
            _data = tagTitleList
                .Select(name => new WebNovelTag
                {
                    Id = Guid.NewGuid(),
                    FallbackName = name
                }).ToList();
        }
    }
}