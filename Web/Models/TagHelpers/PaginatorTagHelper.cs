using Web.Interfaces;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Web.Models.TagHelpers
{
    [HtmlTargetElement("paginator", TagStructure = TagStructure.WithoutEndTag)]
    public class PaginatorTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public PaginatorTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }
        
        public IPageableModel Model { get; set; }
        public string AspAction { get; set; }
        public string AspController { get; set; }
        public int PagesDisplayed { get; set; } = 9;

        [ViewContext]
        public ViewContext ViewContext { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (AspController == null)
            {
                AspController = (string) ViewContext.RouteData.Values["controller"];
            }
            
            if (AspAction == null)
            {
                AspAction = (string) ViewContext.RouteData.Values["action"];
            }
            
            output.SuppressOutput();
            output.Content.AppendHtmlLine("<div class=\"btn-group m-t flex-row justify-center\">");

            var maxPagesPerSide = PagesDisplayed / 2;
            var availableLeftPages = Model.Page - 1;
            var availableRightPages = Model.TotalPages - Model.Page;
            var shouldDisplayFirstPage = availableLeftPages > maxPagesPerSide;
            var shouldDisplayLastPage = availableRightPages > maxPagesPerSide;
            var takePagesFromLeft = shouldDisplayFirstPage
                ? maxPagesPerSide - 2
                : availableLeftPages;
            var takePagesFromRight = shouldDisplayLastPage
                ? maxPagesPerSide - 2
                : availableRightPages;
            var totalDisplayedFromLeft = (shouldDisplayFirstPage ? 2 : 0) + takePagesFromLeft;
            var totalDisplayedFromRight = (shouldDisplayLastPage ? 2 : 0) + takePagesFromRight;
            var totalDisplayedPages = totalDisplayedFromLeft + totalDisplayedFromRight + 1;
            
            while (totalDisplayedPages < PagesDisplayed && totalDisplayedPages < Model.TotalPages)
            {
                if (totalDisplayedFromLeft < maxPagesPerSide)
                {
                    takePagesFromRight++;
                    totalDisplayedFromRight++;
                }
                else if (totalDisplayedFromRight < maxPagesPerSide)
                {
                    takePagesFromLeft++;
                    totalDisplayedFromLeft++;
                }
                else
                {
                    break;
                }
                totalDisplayedPages = totalDisplayedFromLeft + totalDisplayedFromRight + 1;
            }

            output.Content.AppendHtmlLine(GetHtmlLinkTextForPreviousPage());
            
            if (shouldDisplayFirstPage)
            {
                output.Content.AppendHtmlLine(GetHtmlLinkTag(1));
                output.Content.AppendHtmlLine(GetHtmlLinkTag("..."));
            }

            for (var page = Model.Page - takePagesFromLeft; page <= Model.Page + takePagesFromRight; page++)
            {
                output.Content.AppendHtmlLine(GetHtmlLinkTag(page));
            }
            
            if (shouldDisplayLastPage)
            {
                output.Content.AppendHtmlLine(GetHtmlLinkTag("..."));
                output.Content.AppendHtmlLine(GetHtmlLinkTag(Model.TotalPages));
            }
            
            output.Content.AppendHtmlLine(GetHtmlLinkTextForNextPage());
            
            output.Content.AppendHtmlLine("</div>");
        }

        private string GetHtmlLinkTag(int? page, string text)
        {
            if (!page.HasValue)
                return $"<a class=\"btn btn-default disabled\">{text}</a>";
            
            return page == Model.Page
                ? $"<a class=\"btn btn-default text-accent font-bold\">{text}</a>"
                : $"<a class=\"btn btn-default\" href=\"{GetLinkToPage(page.Value)}\">{text}</a>";
        }

        private string GetHtmlLinkTag(int page) => GetHtmlLinkTag(page, page.ToString());
        
        private string GetHtmlLinkTag(string text) => GetHtmlLinkTag(null, text);
        
        private string GetHtmlLinkTextForNextPage()
        {
            int? nextPage = Model.Page >= Model.TotalPages ? Model.TotalPages : Model.Page + 1;
            return GetHtmlLinkTag(nextPage == Model.Page ? null : nextPage, "<i class=\"fas fa-chevron-right\"></i>");
        }
        
        private string GetHtmlLinkTextForPreviousPage()
        {
            int? prevPage = Model.Page <= 1 ? 1 : Model.Page - 1;
            return GetHtmlLinkTag(prevPage == Model.Page ? null : prevPage, "<i class=\"fas fa-chevron-left\"></i>");
        }

        private string GetLinkToPage(int page)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            return urlHelper.Action(AspAction, AspController, new {page});
        }
    }
}