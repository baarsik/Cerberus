using Cerberus.Models.Extensions;
using DataContext.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Cerberus.Models.TagHelpers
{
    [HtmlTargetElement("avatar", TagStructure = TagStructure.WithoutEndTag)]
    public class AvatarTagHelper : TagHelper
    {
        public ApplicationUser User { get; set; }
        
        public string Class { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {   
            output.SuppressOutput();
            var avatarHtml = User.Avatar == "noavatar"
                ? "<i class=\"fas fa-user\"></i>"
                : $"<img src=\"/avatars/{User.Avatar}.png\" alt=\"Avatar\"/>";
            output.Content.AppendHtmlLine($"<div class=\"{Class}\">{avatarHtml}</div>");
        }
    }
}