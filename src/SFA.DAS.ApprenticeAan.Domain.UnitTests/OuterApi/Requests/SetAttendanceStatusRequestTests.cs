using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;

namespace SFA.DAS.ApprenticeAan.Domain.UnitTests.OuterApi.Requests;

public class SetAttendanceStatusRequestTests
{
    [TestCase(true)]
    [TestCase(false)]
    public void SetAttendanceStatus_SetsIsAttending(bool status)
    {
        var sut = new SetAttendanceStatusRequest(status);
        Assert.That(sut.IsAttending, Is.EqualTo(status));
    }
}
