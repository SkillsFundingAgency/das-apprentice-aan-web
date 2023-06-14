using SFA.DAS.ApprenticeAan.Web.UrlHelpers;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.UrlHelpers;

public class MailtoLinkValueTests
{
    [Test]
    public void FromAddressAndSubject_CreatesMailtoLink()
    {
        const string emailAddress = "me@email.com";
        const string subject = "This Is Very Important";
        const string expected = $"mailto:{emailAddress}?subject=This%20Is%20Very%20Important";

        var mailtoLink = MailtoLinkValue.FromAddressAndSubject(emailAddress, subject);

        Assert.That(mailtoLink, Is.EqualTo(expected));
    }
}
