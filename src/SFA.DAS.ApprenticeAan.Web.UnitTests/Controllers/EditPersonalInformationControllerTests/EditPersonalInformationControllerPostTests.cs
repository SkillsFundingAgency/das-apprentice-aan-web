using AutoFixture.NUnit3;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.ApprenticePortal.Authentication.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;

public class EditPersonalInformationControllerPostTests
{
    static readonly string YourAmbassadorProfileUrl = Guid.NewGuid().ToString();

    [Test]
    [MoqInlineAutoData]
    public async Task Post_InvalidModel_ReturnsPersonalDetailView(
        SubmitPersonalDetailModel submitPersonalDetailModel,
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        GetMemberProfileResponse getMemberProfileResponse,
        Mock<IValidator<SubmitPersonalDetailModel>> validatorMock,
        CancellationToken cancellationToken)
    {
        //Arrange
        submitPersonalDetailModel.Biography = null;
        var memberId = Guid.NewGuid();
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        outerApiMock.Setup(o => o.GetMemberProfile(memberId, memberId, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(getMemberProfileResponse));
        EditPersonalInformationController sut = new EditPersonalInformationController(outerApiMock.Object, validatorMock.Object);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddUrlHelperMock()
    .AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());

        //Act
        var response = await sut.Post(submitPersonalDetailModel, cancellationToken);

        //Assert
        Assert.That(response, Is.InstanceOf<ViewResult>());
    }

    [Test]
    [MoqInlineAutoData("")]
    [MoqInlineAutoData("biography")]
    public async Task Post_ValidModel_ReturnsMemberProfileView(
        string biography,
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        CancellationToken cancellationToken)
    {
        //Arrange
        SubmitPersonalDetailModel submitPersonalDetailModel = new()
        {
            RegionId = 5,
            Biography = biography,
            JobTitle = string.Empty,
            ShowBiography = true,
            ShowJobTitle = true,
            OrganisationName = string.Empty,
            UserType = MemberUserType.Apprentice
        };
        var memberId = Guid.NewGuid();
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);

        var validatorMock = new Mock<IValidator<SubmitPersonalDetailModel>>();
        var successfulValidationResult = new ValidationResult();
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<SubmitPersonalDetailModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(successfulValidationResult);

        EditPersonalInformationController sut = new EditPersonalInformationController(outerApiMock.Object, validatorMock.Object);

        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());
        sut.AddUrlHelperMock()
.AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);
        Mock<ITempDataDictionary> tempDataMock = new Mock<ITempDataDictionary>();
        tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.YourAmbassadorProfileSuccessMessage)).Returns(true);
        sut.TempData = tempDataMock.Object;

        //Act
        var response = await sut.Post(submitPersonalDetailModel, cancellationToken);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(response, Is.TypeOf<RedirectToRouteResult>());
            var redirectToAction = (RedirectToRouteResult)response;
            Assert.That(redirectToAction.RouteName, Does.Contain(SharedRouteNames.YourAmbassadorProfile));
        });
    }
}