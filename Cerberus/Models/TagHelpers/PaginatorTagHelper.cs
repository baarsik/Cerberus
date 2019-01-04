using Cerberus.Interfaces;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Cerberus.Models.TagHelpers
{
    [HtmlTargetElement("paginator", TagStructure = TagStructure.WithoutEndTag)]
    public class PaginatorTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public PaginatorTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }
        
        public IPageableViewModel Model { get; set; }
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
                output.Content.AppendHtmlLine(GetHtmlLinkTextForPage(1));
                output.Content.AppendHtmlLine(GetHtmlLinkTextNoHref("..."));
            }

            for (var page = Model.Page - takePagesFromLeft; page <= Model.Page + takePagesFromRight; page++)
            {
                output.Content.AppendHtmlLine(GetHtmlLinkTextForPage(page));
            }
            
            if (shouldDisplayLastPage)
            {
                output.Content.AppendHtmlLine(GetHtmlLinkTextNoHref("..."));
                output.Content.AppendHtmlLine(GetHtmlLinkTextForPage(Model.TotalPages));
            }
            
            output.Content.AppendHtmlLine(GetHtmlLinkTextForNextPage());
            
            output.Content.AppendHtmlLine("</div>");
        }

        private string GetHtmlLinkTextForPage(int page)
        {
            return page == Model.Page
                ? $"<a class=\"btn btn-default text-accent font-bold\">{page}</a>"
                : $"<a class=\"btn btn-default\" href=\"{GetLinkToPage(page)}\">{page}</a>";
        }
        
        private string GetHtmlLinkTextForNextPage()
        {
            var nextPage = Model.Page >= Model.TotalPages ? Model.TotalPages : Model.Page + 1;
            return $"<a class=\"btn btn-default\" href=\"{GetLinkToPage(nextPage)}\"><i class=\"fas fa-chevron-right\"></i></a>";
        }
        
        private string GetHtmlLinkTextForPreviousPage()
        {
            var prevPage = Model.Page <= 1 ? 1 : Model.Page - 1;
            return $"<a class=\"btn btn-default\" href=\"{GetLinkToPage(prevPage)}\"\"><i class=\"fas fa-chevron-left\"></i></a>";
        }
        
        private string GetHtmlLinkTextNoHref(string text)
        {
            return $"<a class=\"btn btn-default\">{text}</a>";
        }
        
        private string GetHtmlLinkTextNoHref(int page)
        {
            return GetHtmlLinkTextNoHref(page.ToString());
        }

        private string GetLinkToPage(int page)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            return urlHelper.Action(AspAction, AspController, new {page = page});
        }
    }
}