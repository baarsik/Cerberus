using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataGrabber.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace DataGrabber
{
    public class Parser
    {
        private int _parsingDataCount = 0;
        private int _parsedDataCount = 0;
        
        private List<WebNovelTagModel> _parsedWebNovelTags = new List<WebNovelTagModel>();

        public async Task Parse()
        {
            await ParseWebNovelTags();
        }

        public async Task Save()
        {
            await File.WriteAllTextAsync("OutputData/web_novel_tags.json", JsonConvert.SerializeObject(_parsedWebNovelTags));
            await File.WriteAllTextAsync("OutputData/web_novel_tags.min.json", JsonConvert.SerializeObject(_parsedWebNovelTags.Select(c => c.Title)));
        }
        
        public DateTime StartDateTime { get; set; } = DateTime.Now;
        
        private async Task ParseWebNovelTags()
        {
            _parsingDataCount++;
            Program.WriteLine("Started parsing web novel tags");
            
            var startTime = DateTime.Now;
            var web = new HtmlWeb();
            var nodeList = new List<HtmlNode>();
            var page = 1;
            while (true)
            {
                var document = await web.LoadFromWebAsync($"https://www.novelupdates.com/list-tags/?st=1&pg={page++}");
                var nodes = document.DocumentNode.SelectNodes("//div[@class='wpb_wrapper']/ul/li/a");
                if (nodes == null)
                    break;

                nodeList.AddRange(nodes);
            }
            
            _parsedWebNovelTags = nodeList
                .Select(c => new WebNovelTagModel
                {
                    Title = c.InnerText,
                    Description = c.GetAttributeValue("title", string.Empty)
                }).ToList();

            _parsedDataCount++;
            Program.WriteLine($"Finished parsing {_parsedWebNovelTags.Count} web novel tag(s) in {(DateTime.Now - startTime).ToString()}");
        }
    }
}