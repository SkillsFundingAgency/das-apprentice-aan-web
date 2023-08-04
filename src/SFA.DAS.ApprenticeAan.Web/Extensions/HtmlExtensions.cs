using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SFA.DAS.ApprenticeAan.Web.Extensions;

public static class HtmlExtensions
{

    public static HtmlString MarkdownToHtml(this IHtmlHelper htmlHelper, string markdownText)
    {
        return !string.IsNullOrEmpty(markdownText)
            ? new HtmlString("" + htmlHelper.Raw(CommonMark.CommonMarkConverter.Convert(markdownText.Replace("\\r", "\r").Replace("\\n", "\n"))) + "")
            : new HtmlString(string.Empty);
    }
}
