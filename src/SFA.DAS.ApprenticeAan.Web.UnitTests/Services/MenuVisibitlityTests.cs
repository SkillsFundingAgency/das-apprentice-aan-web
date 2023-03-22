using FluentAssertions;
using SFA.DAS.ApprenticeAan.Web.Services;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Services;

public class MenuVisibitlityTests
{
    [Test]
    public void ConfirmMyApprenticeshipTitleStatus_ReturnsShowAsConfirmed()
    {
        MenuVisibility sut = new();
        sut.ConfirmMyApprenticeshipTitleStatus().Result.Should().Be(ApprenticePortal.SharedUi.Services.ConfirmMyApprenticeshipTitleStatus.ShowAsConfirmed);
    }

    [Test]
    public void ShowApprenticeFeedback_ReturnsFalse()
    {
        MenuVisibility sut = new();
        sut.ShowApprenticeFeedback().Result.Should().BeFalse();
    }

    [Test]
    public void ShowConfirmMyApprenticeship_ReturnsFalse()
    {
        MenuVisibility sut = new();
        sut.ShowConfirmMyApprenticeship().Result.Should().BeFalse();
    }
}
