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
    public void PopulatesLinkItem()
    {
        var page = 1;
        var linkItems = Enumerable.Range(1, 6);
        PaginationViewModel sut = new(page, 5, 6, BaseUrl);

        sut.LinkItems.Count.Should().Be(6);

        sut.LinkItems.First(s => s.Text == page.ToString()).HasLink.Should().BeFalse();
        sut.LinkItems.Where(s => s.Text != page.ToString()).All(s => s.HasLink).Should().BeTrue();

        foreach (var text in linkItems)
        {
            sut.LinkItems.Exists(s => s.Text == text.ToString());

            if (text != page)
            {
                sut.LinkItems.First(s => s.Text == text.ToString()).Url = BaseUrl + "?Page=" + page.ToString();
            }
            else
            {
                sut.LinkItems.First(s => s.Text == text.ToString()).Url = null;
            }
        }
    }

}
