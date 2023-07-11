﻿namespace SFA.DAS.ApprenticeAan.Web.Models;

public class PaginationViewModel
{
    public const string PreviousText = "« Previous";
    public const string NextText = "Next »";
    public int CurrentPage { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
    public string BaseUrl { get; init; }

    public List<LinkItem> LinkItems { get; set; } = new();


    public PaginationViewModel(int currentPage, int pageSize, int totalPages, string baseUrl)
    {
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalPages = totalPages;
        BaseUrl = baseUrl;

        var startPage = currentPage < 4 ? 1 : currentPage - 2;
        if (totalPages <= 6)
        {
            startPage = 1;
        }
        else
        {
            /// This deals with when the current page is the last page, second to last page, and third to last page
            if (currentPage >= totalPages - 2)
            {
                startPage = currentPage - 5 + (totalPages - currentPage);
            }
        }

        var range = Enumerable.Range(startPage, 6);

        if (currentPage > 1) LinkItems.Add(new(GetUrl(baseUrl, currentPage - 1, pageSize), PreviousText));

        foreach (var r in range)
        {
            var url = r == currentPage ? null : GetUrl(baseUrl, r, pageSize);
            LinkItems.Add(new(url, r.ToString()));
            if (r == totalPages) break;
        }

        if (currentPage < totalPages) LinkItems.Add(new(GetUrl(baseUrl, currentPage + 1, pageSize), NextText));
    }

    public static string GetUrl(string baseUrl, int page, int pageSize)
    {

        var query = $"page={page}&pageSize={pageSize}";
        var hasQueryParameters = baseUrl.Contains('?');
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