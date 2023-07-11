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

    [TestCase(1, 5, 6, 6, 1, false, false)]
    [TestCase(2, 5, 6, 7, 1, true, false)]
    [TestCase(3, 5, 6, 7, 1, true, false)]
    [TestCase(4, 5, 6, 7, 1, true, false)]
    [TestCase(5, 5, 6, 7, 1, true, false)]
    [TestCase(6, 5, 6, 7, 1, true, false)]
    [TestCase(1, 5, 7, 7, 1, false, true)]
    [TestCase(2, 5, 7, 8, 1, true, true)]
    [TestCase(3, 5, 100, 8, 1, true, true)]
    [TestCase(4, 5, 100, 8, 2, true, true)]
    [TestCase(5, 5, 100, 8, 3, true, true)]
    [TestCase(6, 5, 100, 8, 4, true, true)]
    public void PopulatesLinkItem(int currentPage, int pageSize, int totalPages, int totalLinkItems, int firstPageExpected, bool isPreviousExpected, bool isNextExpected)
    {
        var linkItems = Enumerable.Range(firstPageExpected, PaginationViewModel.MaximumPageNumbers);
        PaginationViewModel sut = new(currentPage, pageSize, totalPages, BaseUrl);

        sut.LinkItems.Count.Should().Be(totalLinkItems);

        sut.LinkItems.First(s => s.Text == currentPage.ToString()).HasLink.Should().BeFalse();
        sut.LinkItems.Where(s => s.Text != currentPage.ToString()).All(s => s.HasLink).Should().BeTrue();

        foreach (var text in linkItems)
        {
            //  sut.LinkItems.Exists(s => s.Text == text.ToString()).Should().BeTrue();

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
