using FluentAssertions;
using SFA.DAS.ApprenticeAan.Web.Models;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models;

public class PaginationViewModelTests
{
    public const string BaseUrl = @"http://baseUrl";

    [TestCase("http://baseUrl.com?eventType=1", "http://baseUrl.com?eventType=1&page=2&pageSize=5")]
    [TestCase("https://baseUrl.com", "https://baseUrl.com?page=2&pageSize=5")]
    public void CorrectlyAppendsToTheBaseUrl(string baseUrl, string expectedUrl)
    {
        PaginationViewModel sut = new(1, 5, 2, baseUrl);

        sut.LinkItems.Last().Url.Should().Be(expectedUrl);
    }

    [Test]
    public void ReturnsNoLinksWhenNoTotalPages()
    {
        PaginationViewModel sut = new(1, 5, 0, BaseUrl);
        sut.LinkItems.Count.Should().Be(0);
    }

    [TestCase(1, 2, 3, 1, 2, false, true)]
    [TestCase(1, 6, 7, 1, 6, false, true)]
    [TestCase(1, 7, 7, 1, 6, false, true)]
    [TestCase(2, 6, 8, 1, 6, true, true)]
    [TestCase(2, 7, 8, 1, 6, true, true)]
    [TestCase(3, 6, 8, 1, 6, true, true)]
    [TestCase(3, 7, 8, 1, 6, true, true)]
    [TestCase(3, 100, 8, 1, 6, true, true)]
    [TestCase(4, 6, 8, 1, 6, true, true)]
    [TestCase(4, 7, 8, 2, 7, true, true)]
    [TestCase(4, 100, 8, 2, 7, true, true)]
    [TestCase(5, 6, 8, 1, 6, true, true)]
    [TestCase(5, 7, 8, 2, 7, true, true)]
    [TestCase(5, 100, 8, 3, 8, true, true)]
    [TestCase(6, 6, 7, 1, 6, true, false)]
    [TestCase(6, 100, 8, 4, 9, true, true)]
    [TestCase(7, 7, 7, 2, 7, true, false)]
    [TestCase(8, 12, 8, 6, 11, true, true)]
    [TestCase(8, 100, 8, 6, 11, true, true)]

    public void PopulatesLinkItem(int currentPage, int totalPages, int totalLinkItems, int firstPageExpected, int lastPageExpected, bool isPreviousExpected, bool isNextExpected)
    {
        var pageSize = 5;
        var linkItems = Enumerable.Range(firstPageExpected, lastPageExpected - firstPageExpected + 1);
        PaginationViewModel sut = new(currentPage, pageSize, totalPages, BaseUrl);

        sut.LinkItems.Count.Should().Be(totalLinkItems);

        sut.LinkItems.First(s => s.Text == currentPage.ToString()).HasLink.Should().BeFalse();
        sut.LinkItems.Where(s => s.Text != currentPage.ToString()).All(s => s.HasLink).Should().BeTrue();
        if (!isPreviousExpected)
        {
            sut.LinkItems.First().Text.Should().Be(firstPageExpected.ToString());
        }
        else
        {
            sut.LinkItems.Skip(1).First().Text.Should().Be(firstPageExpected.ToString());
        }

        if (!isNextExpected)
        {
            sut.LinkItems.Last().Text.Should().Be(lastPageExpected.ToString());
        }
        else
        {
            sut.LinkItems[^2].Text.Should().Be(lastPageExpected.ToString());
        }



        foreach (var text in linkItems)
        {
            if (text != currentPage)
            {
                sut.LinkItems.First(s => s.Text == text.ToString()).Url.Should().Be(BaseUrl + "?page=" + text + "&pageSize=" + pageSize);
            }
            else
            {
                sut.LinkItems.First(s => s.Text == text.ToString()).Url.Should().BeNull();
            }
        }

        if (isPreviousExpected)
        {
            sut.LinkItems.First(s => s.Text == PaginationViewModel.PreviousText).Url.Should().Be(BaseUrl + "?page=" + (currentPage - 1) + "&pageSize=" + pageSize);
        }
        else
        {
            sut.LinkItems.Exists(s => s.Text == PaginationViewModel.PreviousText).Should().BeFalse();
        }

        if (isNextExpected)
        {
            sut.LinkItems.First(s => s.Text == PaginationViewModel.NextText).Url.Should().Be(BaseUrl + "?page=" + (currentPage + 1) + "&pageSize=" + pageSize);
        }
        else
        {
            sut.LinkItems.Exists(s => s.Text == PaginationViewModel.NextText).Should().BeFalse();
        }
    }

}
