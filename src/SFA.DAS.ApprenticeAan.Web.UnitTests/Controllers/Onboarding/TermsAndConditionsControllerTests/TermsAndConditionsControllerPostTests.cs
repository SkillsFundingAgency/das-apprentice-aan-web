using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.TermsAndConditionsControllerTests
{
    [TestFixture]
    public class TermsAndConditionsControllerPostTests
    {
        [MoqAutoData]
        public void Post_SetsTempData(
            [Greedy] TermsAndConditionsController sut,
            Mock<ITempDataDictionary> tempDataMock)
        {
            tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.HasSeenTermsAndConditions)).Returns(false);
            sut.TempData = tempDataMock.Object;

            sut.Post();

            tempDataMock.Verify(t => t.Add(TempDataKeys.HasSeenTermsAndConditions, true));
        }

        [MoqAutoData]
        public void Post_RedirectToRouteResult_RedirectsToLineManager(
            [Greedy] TermsAndConditionsController sut,
            Mock<ITempDataDictionary> tempDataMock)
        {
            sut.TempData = tempDataMock.Object;
            tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.HasSeenTermsAndConditions)).Returns(true);

            var result = sut.Post();

            result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.LineManager);
        }
    }
}
