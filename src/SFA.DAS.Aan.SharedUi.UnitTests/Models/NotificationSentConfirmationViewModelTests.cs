using SFA.DAS.Aan.SharedUi.Models;

namespace SFA.DAS.Aan.SharedUi.UnitTests.Models;
public class NotificationSentConfirmationViewModelTests
{
    NotificationSentConfirmationViewModel sut = null!;
    private string networkDirectoryUrl = Guid.NewGuid().ToString();

    [SetUp]
    public void Setup()
    {
        sut = new NotificationSentConfirmationViewModel(networkDirectoryUrl);
    }

    [Test]
    public void NotificationSentConfirmationViewModel_ReturnsExpectedValueForNetworkDirectoryUrl()
    {
        Assert.That(sut.NetworkDirectoryUrl, Is.EqualTo(networkDirectoryUrl));
    }

    [Test]
    public void NotificationSentConfirmationViewModel_ReturnNullValueForNetworkHubLink()
    {
        Assert.That(sut.NetworkHubLink, Is.Null);
    }
}
