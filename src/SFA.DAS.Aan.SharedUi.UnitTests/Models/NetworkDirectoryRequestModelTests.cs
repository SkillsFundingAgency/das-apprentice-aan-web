using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Models.NetworkDirectory;

namespace SFA.DAS.Aan.SharedUi.UnitTests.Models;

public class NetworkDirectoryRequestModelTests
{
    [Test, AutoData]
    public void BuildQueryStringParameters_PopulatesDictionaryBuiltFromModelForKeyword(string? keyword)
    {
        var sut = new NetworkDirectoryRequestModel
        {
            Keyword = keyword
        };

        var result = sut.ToQueryStringParameters();

        result.TryGetValue("keyword", out string[]? keywordResult);
        keywordResult![0].Should().Be(keyword);
    }

    [TestCase(Role.RegionalChair)]
    public void BuildQueryStringParameters_PopulatesDictionaryBuiltFromModelForRegionalChair(Role userRole)
    {
        string regionalChairExpectedResult = "True";
        var sut = new NetworkDirectoryRequestModel
        {
            UserRole = new List<Role> { userRole }
        };

        var result = sut.ToQueryStringParameters();

        result.TryGetValue("isRegionalChair", out var isRegionalChairResult);
        isRegionalChairResult.Should().BeEquivalentTo(regionalChairExpectedResult);
    }

    [Test, AutoData]
    public void BuildQueryStringParameters_PopulatesDictionaryBuiltFromModelForUserRole(List<Role> userRole)
    {
        var sut = new NetworkDirectoryRequestModel
        {
            UserRole = userRole
        };

        var result = sut.ToQueryStringParameters();

        result.TryGetValue("userType", out var userTypesResult);
        userTypesResult!.Length.Should().Be(userRole.Where(userRole => userRole != Role.RegionalChair).ToList().Count);
        userRole.Where(userRole => userRole != Role.RegionalChair).Select(x => x.ToString()).Should().BeEquivalentTo(userTypesResult.ToList());
    }

    [Test, AutoData]
    public void BuildQueryStringParameters_PopulatesDictionaryBuiltFromModelForRegion(List<int> regionIds)
    {
        var sut = new NetworkDirectoryRequestModel
        {
            RegionId = regionIds
        };

        var result = sut.ToQueryStringParameters();

        result.TryGetValue("regionId", out var regionIdsResult);
        regionIdsResult!.Length.Should().Be(regionIds.Count);
        regionIds.Select(x => x.ToString()).Should().BeEquivalentTo(regionIdsResult.ToList());
    }

    [Test, AutoData]
    public void BuildQueryStringParameters_PopulatesDictionaryBuiltFromModelForPage(int? page)
    {
        var sut = new NetworkDirectoryRequestModel
        {
            Page = page
        };

        var result = sut.ToQueryStringParameters();

        result.TryGetValue("page", out string[]? pageResult);
        pageResult![0].Should().Be(page?.ToString());
    }

    [Test, AutoData]
    public void BuildQueryStringParameters_PopulatesDictionaryBuiltFromModelForPageSize(int? pageSize)
    {
        var sut = new NetworkDirectoryRequestModel
        {
            PageSize = pageSize
        };

        var result = sut.ToQueryStringParameters();

        result.TryGetValue("pageSize", out string[]? pageSizeResult);
        pageSizeResult![0].Should().Be(pageSize?.ToString());
    }

    [TestCase(null)]
    [TestCase("directory")]
    public void BuildQueryStringParameters_ConstructsParameterForKeyword(string? keyword)
    {
        var sut = new NetworkDirectoryRequestModel
        {
            Keyword = keyword
        };

        var result = sut.ToQueryStringParameters();

        result.TryGetValue("keyword", out var keywordResult);

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
        var sut = new NetworkDirectoryRequestModel
        {
            Page = page
        };

        var result = sut.ToQueryStringParameters();

        result.TryGetValue("page", out var pageResult);

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
        var sut = new NetworkDirectoryRequestModel
        {
            PageSize = pageSize
        };
        var result = sut.ToQueryStringParameters();

        result.TryGetValue("pageSize", out var pageResult);

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
        var sut = new NetworkDirectoryRequestModel
        {
            IsRegionalChair = isRegionalChair,
            UserRole = userTypes,
            RegionId = regionIds
        };

        var result = sut.ToQueryStringParameters();

        result.TryGetValue("isRegionalChair", out var isRegionalChairResult);
        isRegionalChairResult.Should().BeEquivalentTo(sut.IsRegionalChair.ToString());

        result.TryGetValue("userType", out var userTypesResult);
        userTypesResult!.Length.Should().Be(userTypes.Where(userType => userType != Role.RegionalChair).ToList().Count);
        userTypes.Where(userType => userType != Role.RegionalChair).Select(x => x.ToString()).Should().BeEquivalentTo(userTypesResult.ToList());

        result.TryGetValue("regionId", out var regionIdsResult);
        regionIdsResult!.Length.Should().Be(regionIds.Count);
        regionIds.Select(x => x.ToString()).Should().BeEquivalentTo(regionIdsResult.ToList());

        result.ContainsKey("page").Should().BeFalse();
        result.ContainsKey("pageSize").Should().BeFalse();
    }
}
