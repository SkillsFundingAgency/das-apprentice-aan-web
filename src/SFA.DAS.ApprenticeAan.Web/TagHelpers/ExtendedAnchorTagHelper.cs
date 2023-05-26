using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SFA.DAS.ApprenticeAan.Web.TagHelpers;

[HtmlTargetElement("a", Attributes = "asp-querystring")]
public class ExtendedAnchorTagHelper : AnchorTagHelper
{
    [HtmlAttributeName("asp-querystring")]
    public string QueryString { get; set; }


    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        base.Process(context, output);

        output.Attributes.TryGetAttribute("href", out var attribute);
        output.Attributes.SetAttribute("href", attribute.Value + QueryString);
    }

    public ExtendedAnchorTagHelper(IHtmlGenerator generator) : base(generator)
    {
    }
}
