using AutoFixture;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.CheckYourAnswersControllerTests;

public abstract class CheckYourAnswersControllerGetTestsBase
{
    internal static readonly IEnumerable<int> AddressIds = Enumerable.Range(31, 5);
    internal static List<ProfileModel> GetProfileData()
    {
        var fixture = new Fixture();
        int[] profileIds = new[] { 20, 30 };
        var profileData = fixture.Build<ProfileModel>().WithValues(p => p.Id, profileIds).CreateMany(profileIds.Length).ToList();

        profileData.AddRange(fixture.Build<ProfileModel>().WithValues(p => p.Id, AddressIds.ToArray()).CreateMany(AddressIds.Count()));

        profileData
            .Add(fixture.Build<ProfileModel>()
            .With(p => p.Id, ProfileDataId.HasPreviousEngagement)
            .With(p => p.Value, "true")
            .Create());

        string[] areasOfInterestCategories = new[] { Category.Promotions, Category.Events };
        profileData.AddRange(fixture.Build<ProfileModel>().WithValues(p => p.Id, 1, 2).WithValues(p => p.Category, areasOfInterestCategories).CreateMany(areasOfInterestCategories.Length));
        // Add null values for the same categories for exclusion
        profileData.AddRange(fixture.Build<ProfileModel>().WithValues(p => p.Id, 3, 4).WithValues(p => p.Category, areasOfInterestCategories).Without(p => p.Value).CreateMany(areasOfInterestCategories.Length));
        return profileData;
    }
}
