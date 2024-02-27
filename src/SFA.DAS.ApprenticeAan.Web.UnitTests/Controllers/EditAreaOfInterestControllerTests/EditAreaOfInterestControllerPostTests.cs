using AutoFixture.NUnit3;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models.EditAreaOfInterest;
using SFA.DAS.Aan.SharedUi.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.ApprenticePortal.Authentication.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;

public class EditAreaOfInterestControllerPostTests
{
    static readonly string YourAmbassadorProfileUrl = Guid.NewGuid().ToString();
    private readonly Guid memberId = Guid.NewGuid();

    [Test, MoqAutoData]
    public async Task Post_InvalidCommand_ReturnsEditAreaOfInterestView(
        SubmitAreaOfInterestModel command,
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        GetMemberProfileResponse getMemberProfileResponse,
        Mock<IValidator<SubmitAreaOfInterestModel>> validatorMock,
        CancellationToken cancellationToken)
    {
        //Arrange
        command.FirstSectionInterests = [];
        command.SecondSectionInterests = [];
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        outerApiMock.Setup(o => o.GetMemberProfile(memberId, memberId, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(getMemberProfileResponse));
        EditAreaOfInterestController sut = new(validatorMock.Object, outerApiMock.Object, Mock.Of<ISessionService>())
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } }
        };
        sut.AddUrlHelperMock()
    .AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());

        //Act
        var response = await sut.Post(command, cancellationToken);

        //Assert
        Assert.That(response, Is.InstanceOf<ViewResult>());
    }

    [Test, MoqAutoData]
    public async Task Post_ValidCommand_ReturnsMemberProfileView(
        [Frozen] Mock<IValidator<SubmitAreaOfInterestModel>> validatorMock,
        [Frozen] Mock<ITempDataDictionary> tempDataMock,
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        List<SelectProfileViewModel> selectProfileViewModels,
        CancellationToken cancellationToken)
    {
        //Arrange
        SubmitAreaOfInterestModel command = new()
        {
            FirstSectionInterests = selectProfileViewModels,
            SecondSectionInterests = selectProfileViewModels
        };
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);

        var successfulValidationResult = new ValidationResult();
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<SubmitAreaOfInterestModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(successfulValidationResult);

        EditAreaOfInterestController sut = new(validatorMock.Object, outerApiMock.Object, Mock.Of<ISessionService>())
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } }
        };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());
        sut.AddUrlHelperMock()
.AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);

        tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.YourAmbassadorProfileSuccessMessage)).Returns(true);
        sut.TempData = tempDataMock.Object;

        //Act
        var response = await sut.Post(command, cancellationToken);
        var redirectToAction = (RedirectToRouteResult)response;

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(response, Is.TypeOf<RedirectToRouteResult>());
            Assert.That(redirectToAction.RouteName, Does.Contain(SharedRouteNames.YourAmbassadorProfile));
        });
    }
}