using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Extensions;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Extensions;
public class SessionServiceExtensionsTests
{
    [Test, AutoData]
    public void GetMemberId_ReturnsMemberId(Guid memberId)
    {
        var sessionServiceMock = new Mock<ISessionService>();
        sessionServiceMock.Setup(x => x.Get(Constants.SessionKeys.Member.MemberId)).Returns(memberId.ToString);
        var actualMemberId = sessionServiceMock.Object.GetMemberId();
        actualMemberId.Should().Be(memberId.ToString());
    }

    [Test]
    public void GetMemberId_ReturnsNoMemberId()
    {
        var sessionServiceMock = new Mock<ISessionService>();
        var actualMemberId = sessionServiceMock.Object.GetMemberId();
        actualMemberId.Should().Be(Guid.Empty);
    }

    [TestCase(true, true, true)]
    [TestCase(false, true, false)]
    [TestCase(true, false, false)]
    [TestCase(false, false, false)]
    public void IsMemberLive(bool memberExists, bool memberIsLive, bool expectedResult)
    {
        var memberId = Guid.NewGuid();

        var sessionServiceMock = new Mock<ISessionService>();

        if (memberExists)
        {
            sessionServiceMock.Setup(x => x.Get(Constants.SessionKeys.Member.MemberId)).Returns(memberId.ToString);
        }

        var memberStatus = MemberStatus.Withdrawn.ToString();
        if (memberIsLive)
        {
            memberStatus = MemberStatus.Live.ToString();
        }

        sessionServiceMock.Setup(x => x.Get(Constants.SessionKeys.Member.Status)).Returns(memberStatus);

        var isMemberLive = sessionServiceMock.Object.GetMemberStatus() == MemberStatus.Live;
        isMemberLive.Should().Be(expectedResult);
    }
}