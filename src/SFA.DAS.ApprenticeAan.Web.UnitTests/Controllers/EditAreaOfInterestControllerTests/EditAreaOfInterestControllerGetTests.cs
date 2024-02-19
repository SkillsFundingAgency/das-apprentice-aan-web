using AutoFixture.NUnit3;
using FluentAssertions.Execution;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.Aan.SharedUi.Models.EditAreaOfInterest;
using SFA.DAS.Aan.SharedUi.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.ApprenticePortal.Authentication.TestHelpers;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;

public class EditAreaOfInterestControllerGetTests
{
    static readonly string YourAmbassadorProfileUrl = Guid.NewGuid().ToString();
    private readonly static Guid memberId = Guid.NewGuid();
    private EditAreaOfInterestController sut = null!;
    private Mock<IOuterApiClient> _outerApiMock = null!;
    private Mock<IValidator<SubmitAreaOfInterestModel>> _validatorMock = null!;
    private GetMemberProfileResponse memberProfileResponse = null!;
    private readonly List<Profile> profiles =
    [
        new Profile { Id = 1, Description = "Networking at events in person", Category = AreaOfInterestTitleConstant.FirstSectionTitleForApprentice, Ordering = 1 },
        new Profile { Id = 2, Description = "Presenting at events in person", Category = AreaOfInterestTitleConstant.FirstSectionTitleForApprentice, Ordering = 2 },
        new Profile { Id = 10, Description = "Carrying out and writing up case studies", Category = AreaOfInterestTitleConstant.SecondSectionTitleForApprentice, Ordering = 1 },
        new Profile { Id = 11, Description = "Designing and creating marketing materials to champion the network", Category = AreaOfInterestTitleConstant.SecondSectionTitleForApprentice, Ordering = 2 }
    ];

    [Test]
    public void Get_ReturnsEditAreaOfInterestViewModel()
    {
        //Arrange
        HappyPathSetUp();

        //Act
        var result = (ViewResult)sut.Get(new CancellationToken());

        //Assert
        Assert.That(result.Model, Is.TypeOf<EditAreaOfInterestViewModel>());
    }

    [Test]
    public void Index_InvokesOuterApiClientGetMemberProfile()
    {
        //Arrange
        HappyPathSetUp();

        //Act
        var result = sut.GetAreaOfInterests(CancellationToken.None);

        //Assert
        _outerApiMock.Verify(o => o.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public void Index_InvokesOuterApiClientGetProfilesByUserType()
    {
        //Arrange
        HappyPathSetUp();

        //Act
        var result = sut.GetAreaOfInterests(CancellationToken.None);

        //Assert
        _outerApiMock.Verify(o => o.GetProfilesByUserType(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public void Index_ReturnsEditAreaOfInterestView()
    {
        //Arrange
        HappyPathSetUp();

        //Act
        var result = sut.Get(new CancellationToken());

        //Assert
        Assert.Multiple(() =>
        {
            var viewResult = result as ViewResult;
            Assert.That(viewResult!.ViewName, Does.Contain(SharedRouteNames.EditAreaOfInterest));
        });
    }

    [Test, AutoData]
    public void SelectProfileViewModelMapping_ReturnsSelectProfileViewModelList(bool profileValue, IEnumerable<MemberProfile> memberProfiles)
    {
        //Arrange
        EditPersonalInformationViewModel editPersonalInformationViewModel = new();
        List<SelectProfileViewModel> selectProfileViewModels = [];
        memberProfiles.FirstOrDefault()!.Value = profileValue.ToString();
        List<Profile> profiles =
        [
            new Profile { Id = memberProfiles.FirstOrDefault()!.ProfileId }
        ];

        //Act
        var sut = EditAreaOfInterestController.SelectProfileViewModelMapping(profiles, memberProfiles);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut, Is.InstanceOf(selectProfileViewModels.GetType()));
            Assert.That(sut, Has.Count.EqualTo(1));
            Assert.That(sut[0].Id, Is.EqualTo(profiles.ToArray()[0].Id));
            Assert.That(sut[0].Description, Is.EqualTo(profiles.ToArray()[0].Description));
            Assert.That(sut[0].Category, Is.EqualTo(profiles.ToArray()[0].Category));
            Assert.That(sut[0].Ordering, Is.EqualTo(profiles.ToArray()[0].Ordering));
            Assert.That(sut[0].IsSelected, Is.EqualTo(profileValue));
        });
    }

    [Test]
    public void Index_EditAreaOfInterestViewModel_ShouldHaveNullValueForNetworkHubLink()
    {
        // Arrange
        HappyPathSetUp();

        // Act
        var result = sut.Get(new CancellationToken());
        var viewResult = result as ViewResult;
        var viewModel = viewResult!.Model as EditAreaOfInterestViewModel;

        // Assert
        Assert.That(viewModel!.NetworkHubLink, Is.Null);
    }

    [Test]
    public void Index_EditAreaOfInterestViewModel_ShouldHaveExpectedValueForTitles()
    {
        // Arrange
        HappyPathSetUp();

        // Act
        var result = sut.Get(CancellationToken.None);
        var viewResult = result as ViewResult;
        var viewModel = viewResult!.Model as EditAreaOfInterestViewModel;

        // Assert
        using (new AssertionScope())
        {
            Assert.Multiple(() =>
            {
                Assert.That(viewModel!.FirstSectionTitle, Is.EqualTo(AreaOfInterestTitleConstant.FirstSectionTitleForApprentice));
                Assert.That(viewModel!.SecondSectionTitle, Is.EqualTo(AreaOfInterestTitleConstant.SecondSectionTitleForApprentice));
            });
        }
    }

    [Test]
    public void Index_EditAreaOfInterestViewModel_ShouldHaveExpectedValueForYourAmbassadorProfileUrl()
    {
        // Arrange
        HappyPathSetUp();

        // Act
        var result = sut.Get(CancellationToken.None);
        var viewResult = result as ViewResult;
        var viewModel = viewResult!.Model as EditAreaOfInterestViewModel;

        // Assert
        using (new AssertionScope())
        {
            Assert.That(viewModel!.YourAmbassadorProfileUrl, Is.EqualTo(YourAmbassadorProfileUrl));
        }
    }

    [TearDown]
    public void TearDown()
    {
        sut?.Dispose();
    }

    private void SetUpControllerWithContext()
    {
        sut = new(_validatorMock.Object, _outerApiMock.Object, Mock.Of<ISessionService>());
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());
        sut.AddUrlHelperMock()
.AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);
    }

    private void HappyPathSetUp()
    {
        _outerApiMock = new();
        _validatorMock = new();
        SetUpControllerWithContext();
        memberProfileResponse = new()
        {
            Profiles = new List<MemberProfile>(),
            Preferences = new List<MemberPreference>(),
            RegionId = 1,
            OrganisationName = string.Empty
        };

        _outerApiMock.Setup(o => o.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
               .Returns(Task.FromResult(memberProfileResponse));
        _outerApiMock.Setup(o => o.GetProfilesByUserType(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new GetProfilesResult() { Profiles = profiles }));
    }
}