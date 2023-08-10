using SFA.DAS.Aan.SharedUi.UrlHelpers;

namespace SFA.DAS.Aan.SharedUi.UnitTests.UrlHelpers;

public class MailToLinkValueTests
{
    [Test]
    public void FromAddressAndSubject_CreatesMailToLink()
    {
        const string emailAddress = "me@email.com";
        const string subject = "This Is Very Important";
        const string expected = $"mailto:{emailAddress}?subject=This%20Is%20Very%20Important";

        var mailToLink = MailToLinkValue.FromAddressAndSubject(emailAddress, subject);

        Assert.That(mailToLink, Is.EqualTo(expected));
    }
}
