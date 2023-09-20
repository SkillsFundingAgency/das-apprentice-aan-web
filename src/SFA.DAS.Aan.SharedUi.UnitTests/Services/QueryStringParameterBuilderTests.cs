using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Models.NetworkDirectory;
using SFA.DAS.Aan.SharedUi.Models.NetworkEvents;
using SFA.DAS.ApprenticeAan.Web.Services;

namespace SFA.DAS.Aan.SharedUi.UnitTests.Services;

public class QueryStringParameterBuilderTests
{

    [Test, AutoData]
    public void Builder_PopulatesDictionaryBuiltFromModel(string? keyword, DateTime? fromDate, DateTime? toDate,
        List<EventFormat> eventFormats, List<int> calendarIds, List<int> regionIds, int? page, int? pageSize)
    {
        var request = new GetNetworkEventsRequest
        {
            Keyword = keyword,
            FromDate = fromDate,
            ToDate = toDate,
            EventFormat = eventFormats,
            CalendarId = calendarIds,
            RegionId = regionIds,
            Page = page,
            PageSize = pageSize
        };

        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(request);

        parameters.TryGetValue("keyword", out string[]? keywordResult);
        keywordResult![0].Should().Be(keyword);

        parameters.TryGetValue("fromDate", out string[]? fromDateResult);
        fromDateResult![0].Should().Be(fromDate?.ToString("yyyy-MM-dd"));

        parameters.TryGetValue("toDate", out var toDateResult);
        toDateResult![0].Should().Be(toDate?.ToString("yyyy-MM-dd"));

        parameters.TryGetValue("eventFormat", out var eventFormatResult);
        eventFormatResult!.Length.Should().Be(eventFormats.Count);
        eventFormats.Select(x => x.ToString()).Should().BeEquivalentTo(eventFormatResult.ToList());

        parameters.TryGetValue("calendarId", out var calendarIdsResult);
        calendarIdsResult!.Length.Should().Be(calendarIds.Count);
        calendarIds.Select(x => x.ToString()).Should().BeEquivalentTo(calendarIdsResult.ToList());

        parameters.TryGetValue("regionId", out var regionIdsResult);
        regionIdsResult!.Length.Should().Be(regionIds.Count);
        regionIds.Select(x => x.ToString()).Should().BeEquivalentTo(regionIdsResult.ToList());

        parameters.TryGetValue("page", out string[]? pageResult);
        pageResult![0].Should().Be(page?.ToString());

        parameters.TryGetValue("pageSize", out string[]? pageSizeResult);
        pageSizeResult![0].Should().Be(pageSize?.ToString());
    }

    [TestCase(null)]
    [TestCase("event")]
    public void Builder_ConstructParameters_Keyword(string? keyword)
    {
        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(new GetNetworkEventsRequest
        {
            Keyword = keyword
        });
        parameters.TryGetValue("keyword", out var keywordResult);
        if (keyword != null)
        {
            keywordResult![0].Should().Be(keyword);
        }
        else
        {
            keywordResult.Should().BeNull();
        }
    }

    [TestCase(null)]
    [TestCase("2030-06-01")]
    public void Builder_ConstructParameters_FromDate(DateTime? fromDate)
    {
        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(new GetNetworkEventsRequest
        {
            FromDate = fromDate
        });
        parameters.TryGetValue("fromDate", out var fromDateResult);
        if (fromDate != null)
        {
            fromDateResult![0].Should().Be(fromDate?.ToString("yyyy-MM-dd"));
        }
        else
        {
            fromDateResult.Should().BeNull();
        }
    }

    [TestCase(null)]
    [TestCase("2030-06-01")]
    public void Builder_ConstructParameters_ToDate(DateTime? toDate)
    {
        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(new GetNetworkEventsRequest
        {
            ToDate = toDate
        });
        parameters.TryGetValue("toDate", out var toDateResult);
        if (toDate != null)
        {
            toDateResult![0].Should().Be(toDate?.ToString("yyyy-MM-dd"));
        }
        else
        {
            toDateResult.Should().BeNull();
        }
    }

    [TestCase(null)]
    [TestCase(3)]
    public void Builder_ConstructParameters_ToPage(int? page)
    {
        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(new GetNetworkEventsRequest
        {
            Page = page
        });

        parameters.TryGetValue("page", out var pageResult);
        if (pageResult != null)
        {
            pageResult![0].Should().Be(page?.ToString());
        }
        else
        {
            pageResult.Should().BeNull();
        }
    }

    [TestCase(null)]
    [TestCase(6)]
    public void Builder_ConstructParameters_ToPageSize(int? pageSize)
    {
        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(new GetNetworkEventsRequest
        {
            PageSize = pageSize
        });

        parameters.TryGetValue("pageSize", out var pageResult);
        if (pageResult != null)
        {
            pageResult![0].Should().Be(pageSize?.ToString());
        }
        else
        {
            pageResult.Should().BeNull();
        }
    }

    [Test, AutoData]
    public void Builder_PopulatesParametersFromRequestWithNullPageAndPageSize(DateTime? fromDate, DateTime? toDate, List<EventFormat> eventFormats, List<int> calendarIds, List<int> regionIds)
    {
        var request = new GetNetworkEventsRequest
        {
            FromDate = fromDate,
            ToDate = toDate,
            EventFormat = eventFormats,
            CalendarId = calendarIds,
            RegionId = regionIds
        };

        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(request);

        parameters.TryGetValue("fromDate", out string[]? fromDateResult);
        fromDateResult![0].Should().Be(fromDate?.ToString("yyyy-MM-dd"));

        parameters.TryGetValue("toDate", out var toDateResult);
        toDateResult![0].Should().Be(toDate?.ToString("yyyy-MM-dd"));

        parameters.TryGetValue("eventFormat", out var eventFormatResult);
        eventFormatResult!.Length.Should().Be(eventFormats.Count);
        eventFormats.Select(x => x.ToString()).Should().BeEquivalentTo(eventFormatResult.ToList());

        parameters.TryGetValue("calendarId", out var calendarIdsResult);
        calendarIdsResult!.Length.Should().Be(calendarIds.Count);
        calendarIds.Select(x => x.ToString()).Should().BeEquivalentTo(calendarIdsResult.ToList());

        parameters.TryGetValue("regionId", out var regionIdsResult);
        regionIdsResult!.Length.Should().Be(regionIds.Count);
        regionIds.Select(x => x.ToString()).Should().BeEquivalentTo(regionIdsResult.ToList());

        parameters.ContainsKey("page").Should().BeFalse();
        parameters.ContainsKey("pageSize").Should().BeFalse();
    }

    [Test, AutoData]
    public void BuildQueryStringParameters_PopulatesDictionaryBuiltFromModelForKeyword(string? keyword)
    {
        var request = new NetworkDirectoryRequestModel
        {
            Keyword = keyword
        };

        var sut = QueryStringParameterBuilder.BuildQueryStringParameters(request);

        sut.TryGetValue("keyword", out string[]? keywordResult);
        keywordResult![0].Should().Be(keyword);
    }

    [TestCase(Role.RegionalChair)]
    public void BuildQueryStringParameters_PopulatesDictionaryBuiltFromModelForRegionalChair(Role userRole)
    {
        string regionalChairExpectedResult = "True";
        var request = new NetworkDirectoryRequestModel
        {
            UserRole = new List<Role> { userRole }
        };

        var sut = QueryStringParameterBuilder.BuildQueryStringParameters(request);

        sut.TryGetValue("isRegionalChair", out var isRegionalChairResult);
        isRegionalChairResult.Should().BeEquivalentTo(regionalChairExpectedResult);
    }

    [Test, AutoData]
    public void BuildQueryStringParameters_PopulatesDictionaryBuiltFromModelForUserRole(List<Role> userRole)
    {
        var request = new NetworkDirectoryRequestModel
        {
            UserRole = userRole
        };

        var sut = QueryStringParameterBuilder.BuildQueryStringParameters(request);

        sut.TryGetValue("userType", out var userTypesResult);
        userTypesResult!.Length.Should().Be(userRole.Where(userRole => userRole != Role.RegionalChair).ToList().Count);
        userRole.Where(userRole => userRole != Role.RegionalChair).Select(x => x.ToString()).Should().BeEquivalentTo(userTypesResult.ToList());
    }

    [Test, AutoData]
    public void BuildQueryStringParameters_PopulatesDictionaryBuiltFromModelForRegion(List<int> regionIds)
    {
        var request = new NetworkDirectoryRequestModel
        {
            RegionId = regionIds
        };

        var sut = QueryStringParameterBuilder.BuildQueryStringParameters(request);

        sut.TryGetValue("regionId", out var regionIdsResult);
        regionIdsResult!.Length.Should().Be(regionIds.Count);
        regionIds.Select(x => x.ToString()).Should().BeEquivalentTo(regionIdsResult.ToList());
    }

    [Test, AutoData]
    public void BuildQueryStringParameters_PopulatesDictionaryBuiltFromModelForPage(int? page)
    {
        var request = new NetworkDirectoryRequestModel
        {
            Page = page
        };

        var sut = QueryStringParameterBuilder.BuildQueryStringParameters(request);

        sut.TryGetValue("page", out string[]? pageResult);
        pageResult![0].Should().Be(page?.ToString());
    }

    [Test, AutoData]
    public void BuildQueryStringParameters_PopulatesDictionaryBuiltFromModelForPageSize(int? pageSize)
    {
        var request = new NetworkDirectoryRequestModel
        {
            PageSize = pageSize
        };

        var sut = QueryStringParameterBuilder.BuildQueryStringParameters(request);

        sut.TryGetValue("pageSize", out string[]? pageSizeResult);
        pageSizeResult![0].Should().Be(pageSize?.ToString());
    }

    [TestCase(null)]
    [TestCase("directory")]
    public void BuildQueryStringParameters_ConstructsParameterForKeyword(string? keyword)
    {
        var sut = QueryStringParameterBuilder.BuildQueryStringParameters(new NetworkDirectoryRequestModel
        {
            Keyword = keyword
        });

        sut.TryGetValue("keyword", out var keywordResult);

        if (keyword != null)
        {
            keywordResult![0].Should().Be(keyword);
        }
        else
        {
            keywordResult.Should().BeNull();
        }
    }

    [TestCase(null)]
    [TestCase(3)]
    public void NetworkDirectoryBuilder_ConstructsParameterForPage(int? page)
    {
        var sut = QueryStringParameterBuilder.BuildQueryStringParameters(new NetworkDirectoryRequestModel
        {
            Page = page
        });

        sut.TryGetValue("page", out var pageResult);

        if (pageResult != null)
        {
            pageResult![0].Should().Be(page?.ToString());
        }
        else
        {
            pageResult.Should().BeNull();
        }
    }

    [TestCase(null)]
    [TestCase(6)]
    public void NetworkDirectoryBuilder_ConstructsParameterForPageSize(int? pageSize)
    {
        var sut = QueryStringParameterBuilder.BuildQueryStringParameters(new NetworkDirectoryRequestModel
        {
            PageSize = pageSize
        });

        sut.TryGetValue("pageSize", out var pageResult);

        if (pageResult != null)
        {
            pageResult![0].Should().Be(pageSize?.ToString());
        }
        else
        {
            pageResult.Should().BeNull();
        }
    }

    [Test, AutoData]
    public void BuildQueryStringParameters_PopulatesParametersFromRequestWithNullPageAndPageSize(bool? isRegionalChair, List<Role> userTypes, List<int> regionIds)
    {
        var request = new NetworkDirectoryRequestModel
        {
            IsRegionalChair = isRegionalChair,
            UserRole = userTypes,
            RegionId = regionIds
        };

        var sut = QueryStringParameterBuilder.BuildQueryStringParameters(request);

        sut.TryGetValue("isRegionalChair", out var isRegionalChairResult);
        isRegionalChairResult.Should().BeEquivalentTo(request.IsRegionalChair.ToString());

        sut.TryGetValue("userType", out var userTypesResult);
        userTypesResult!.Length.Should().Be(userTypes.Where(userType => userType != Role.RegionalChair).ToList().Count);
        userTypes.Where(userType => userType != Role.RegionalChair).Select(x => x.ToString()).Should().BeEquivalentTo(userTypesResult.ToList());

        sut.TryGetValue("regionId", out var regionIdsResult);
        regionIdsResult!.Length.Should().Be(regionIds.Count);
        regionIds.Select(x => x.ToString()).Should().BeEquivalentTo(regionIdsResult.ToList());

        sut.ContainsKey("page").Should().BeFalse();
        sut.ContainsKey("pageSize").Should().BeFalse();
    }


}