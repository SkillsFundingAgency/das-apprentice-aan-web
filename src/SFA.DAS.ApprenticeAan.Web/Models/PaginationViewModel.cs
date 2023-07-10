namespace SFA.DAS.ApprenticeAan.Web.Models;

public class PaginationViewModel
{
    public const string PreviousText = "<< Previous";
    public const string NextText = "Next >>";
    public int Page { get; init; } //Current page
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
    public int TotalCount { get; set; }
    public string BaseUrl { get; init; }

    public List<LinkItem> LinkItems { get; set; } = new();


    public PaginationViewModel(int currentPage, int pageSize, int totalPages, string baseUrl)
    {
        Page = currentPage;
        PageSize = pageSize;
        TotalPages = totalPages;
        BaseUrl = baseUrl;
        //<< Previous 4 5 6 7 8 9 Next >>

        var startPage = currentPage < 4 ? 1 : currentPage - 2;

        var endPage = startPage + 5;

        var range = Enumerable.Range(startPage, endPage);

        if (startPage > 1) LinkItems.Add(new(GetUrl(baseUrl, startPage - 1, pageSize), PreviousText));

        foreach (var r in range)
        {
            string? url = r == currentPage ? null : GetUrl(baseUrl, r, pageSize);
            LinkItems.Add(new(url, r.ToString()));
            if (r == totalPages) break;
        }

        if (endPage < totalPages) LinkItems.Add(new(GetUrl(baseUrl, endPage + 1, pageSize), NextText));
    }


    public static string GetUrl(string baseUrl, int page, int pageSize)
    {

        var query = $"page={page}&pageSize={pageSize}";
        var hasQueryParameters = baseUrl.Contains("?");
        var queryToAppend = hasQueryParameters ? $"&{query}" : $"?{query}";

        return $"{baseUrl}{queryToAppend}";
    }


    public class LinkItem
    {
        public string? Url { get; set; }
        public string Text { get; set; }
        public bool HasLink => !string.IsNullOrEmpty(Url);
        public LinkItem(string? url, string text)
        {
            Url = url;
            Text = text;
        }
    }
}


/*
 * foreach(var link in LinkItems)
 *   if(link.HasLink)
 *     <a href=link.Url>link.Text</a>
 *   else
 *     <div>link.Text<div>
 * 
 * 
 * 
 * 
 */