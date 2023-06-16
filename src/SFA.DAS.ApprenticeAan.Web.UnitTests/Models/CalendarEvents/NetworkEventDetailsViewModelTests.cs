using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;
using SFA.DAS.ApprenticeAan.Web.UrlHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models.CalendarEvents;

public class NetworkEventDetailsViewModelTests
{
    [Test, RecursiveMoqAutoData]
    public void Constructor_ReceivesOnlineEvent_SetsAllPropertiesExceptLocationDetails(CalendarEvent source)
    {
        source.EventFormat = EventFormat.Online;

        var sut = new NetworkEventDetailsViewModel(source, Guid.NewGuid(), "someapikey", "someprivatesignature");

        Assert.Multiple(() =>
        {
            Assert.That(sut.CalendarEventId, Is.EqualTo(source.CalendarEventId));
            Assert.That(sut.CalendarName, Is.EqualTo(source.CalendarName));
            Assert.That(sut.EventFormat, Is.EqualTo(source.EventFormat));
            Assert.That(sut.StartDate, Is.EqualTo(source.StartDate.ToString("dddd, d MMMM yyyy")));
            Assert.That(sut.EndDate, Is.EqualTo(source.EndDate.ToString("dddd, d MMMM yyyy")));
            Assert.That(sut.Title, Is.EqualTo(source.Title));
            Assert.That(sut.Description, Is.EqualTo(source.Description));
            Assert.That(sut.Summary, Is.EqualTo(source.Summary));
            Assert.That(sut.LocationDetails?.Location, Is.Null);
            Assert.That(sut.LocationDetails?.Postcode, Is.Null);
            Assert.That(sut.LocationDetails?.Longitude, Is.Null);
            Assert.That(sut.LocationDetails?.Latitude, Is.Null);
            Assert.That(sut.LocationDetails?.Distance, Is.Null);
            Assert.That(sut.EventLink, Is.EqualTo(source.EventLink));
            Assert.That(sut.ContactName, Is.EqualTo(source.ContactName));
            Assert.That(sut.ContactEmail, Is.EqualTo(source.ContactEmail));
            Assert.That(sut.CancelReason, Is.EqualTo(source.CancelReason));
            Assert.That(sut.Attendees, Is.EqualTo(source.Attendees));
            Assert.That(sut.AttendeeCount, Is.EqualTo(source.Attendees.Count));
            Assert.That(sut.EventGuests, Is.EqualTo(source.EventGuests));
        });
    }

    [Test, RecursiveMoqAutoData]
    public void Constructor_ReceivesHybridEvent_SetsAllProperties(CalendarEvent source)
    {
        source.EventFormat = EventFormat.Hybrid;

        var sut = new NetworkEventDetailsViewModel(source, Guid.NewGuid(), "someapikey", "someprivatesignature");

        DoAssertsForInPersonAndHybridEvents(source, sut);
    }

    [Test, RecursiveMoqAutoData]
    public void Constructor_ReceivesInPersonEvent_SetsAllProperties(CalendarEvent source)
    {
        source.EventFormat = EventFormat.InPerson;

        var sut = new NetworkEventDetailsViewModel(source, Guid.NewGuid(), "someapikey", "someprivatesignature");

        DoAssertsForInPersonAndHybridEvents(source, sut);
    }

    [Test]
    public void AttendeesContainsCurrentMemberId_IsSignedUpIsTrue()
    {
        var memberId = Guid.NewGuid();
        var source = new CalendarEvent();
        source.Attendees.Add(new Attendee() { MemberId = memberId });

        var sut = new NetworkEventDetailsViewModel(source, memberId, "someapikey", "someprivatesignature");

        Assert.That(sut.IsSignedUp, Is.True);
    }

    [Test]
    public void AttendeesDoesNotContainCurrentMemberId_IsSignedUpIsFalse()
    {
        var memberId = Guid.NewGuid();
        var source = new CalendarEvent();

        var sut = new NetworkEventDetailsViewModel(source, memberId, "someapikey", "someprivatesignature");

        Assert.That(sut.IsSignedUp, Is.False);
    }

    [Test]
    public void GetPartialViewName_EventFormatIsOnline_RetrievesOnlinePartialView()
    {
        var source = new CalendarEvent() { EventFormat = EventFormat.Online };
        var sut = new NetworkEventDetailsViewModel(source, Guid.NewGuid(), "someapikey", "someprivatesignature");

        Assert.That(sut.PartialViewName, Is.EqualTo("_OnlineEventPartial.cshtml"));
    }

    [Test]
    public void GetPartialViewName_EventFormatIsInPerson_RetrievesInPersonPartialView()
    {
        var source = new CalendarEvent() { EventFormat = EventFormat.InPerson };
        var sut = new NetworkEventDetailsViewModel(source, Guid.NewGuid(), "someapikey", "someprivatesignature");

        Assert.That(sut.PartialViewName, Is.EqualTo("_InPersonEventPartial.cshtml"));
    }

    [Test]
    public void GetPartialViewName_EventFormatIsHybrid_RetrievesInPersonPartialView()
    {
        var source = new CalendarEvent() { EventFormat = EventFormat.Hybrid };
        var sut = new NetworkEventDetailsViewModel(source, Guid.NewGuid(), "someapikey", "someprivatesignature");

        Assert.That(sut.PartialViewName, Is.EqualTo("_HybridEventPartial.cshtml"));
    }

    [Test]
    public void GetPartialViewName_EventFormatIsUnknown_Throws()
    {
        var source = new CalendarEvent() { EventFormat = (EventFormat)3 };
        var sut = new NetworkEventDetailsViewModel(source, Guid.NewGuid(), "someapikey", "someprivatesignature");

        Assert.That(() => sut.PartialViewName, Throws.InstanceOf<NotImplementedException>());
    }

    [Test, MoqAutoData]
    public void StartTime_ReturnsTimePortionOfStartDate(CalendarEvent calendarEvent)
    {
        var sut = new NetworkEventDetailsViewModel(calendarEvent, Guid.NewGuid(), "someapikey", "someprivatesignature");

        Assert.That(sut.StartTime, Is.EqualTo(calendarEvent.StartDate.ToString("h:mm tt")));
    }

    [Test, MoqAutoData]
    public void EndTime_ReturnsTimePortionOfEndDate(CalendarEvent calendarEvent)
    {
        var sut = new NetworkEventDetailsViewModel(calendarEvent, Guid.NewGuid(), "someapikey", "someprivatesignature");

        Assert.That(sut.EndTime, Is.EqualTo(calendarEvent.EndDate.ToString("h:mm tt")));
    }

    [Test, MoqAutoData]
    public void EmailLink_ReturnsUrlEscapedMailtoLink(CalendarEvent calendarEvent)
    {
        calendarEvent.ContactEmail = "me@email.com";
        calendarEvent.Description = "This Is A Description";

        string expected = $"mailto:{calendarEvent.ContactEmail}?subject=This%20Is%20A%20Description";

        var sut = new NetworkEventDetailsViewModel(calendarEvent, Guid.NewGuid(), "someapikey", "someprivatesignature");

        Assert.That(sut.EmailLink, Is.EqualTo(expected));
    }

    private static void DoAssertsForInPersonAndHybridEvents(CalendarEvent source, NetworkEventDetailsViewModel sut)
    {
        Assert.Multiple(() =>
        {
            Assert.That(sut.CalendarEventId, Is.EqualTo(source.CalendarEventId));
            Assert.That(sut.CalendarName, Is.EqualTo(source.CalendarName));
            Assert.That(sut.EventFormat, Is.EqualTo(source.EventFormat));
            Assert.That(sut.StartDate, Is.EqualTo(source.StartDate.ToString("dddd, d MMMM yyyy")));
            Assert.That(sut.EndDate, Is.EqualTo(source.EndDate.ToString("dddd, d MMMM yyyy")));
            Assert.That(sut.Title, Is.EqualTo(source.Title));
            Assert.That(sut.Description, Is.EqualTo(source.Description));
            Assert.That(sut.Summary, Is.EqualTo(source.Summary));
            Assert.That(sut.LocationDetails?.Location, Is.EqualTo(source.Location));
            Assert.That(sut.LocationDetails?.Postcode, Is.EqualTo(source.Postcode));
            Assert.That(sut.LocationDetails?.Longitude, Is.EqualTo(source.Longitude));
            Assert.That(sut.LocationDetails?.Latitude, Is.EqualTo(source.Latitude));
            Assert.That(sut.LocationDetails?.Distance, Is.EqualTo(source.Distance));
            Assert.That(sut.EventLink, Is.EqualTo(source.EventLink));
            Assert.That(sut.ContactName, Is.EqualTo(source.ContactName));
            Assert.That(sut.ContactEmail, Is.EqualTo(source.ContactEmail));
            Assert.That(sut.CancelReason, Is.EqualTo(source.CancelReason));
            Assert.That(sut.Attendees, Is.EqualTo(source.Attendees));
            Assert.That(sut.AttendeeCount, Is.EqualTo(source.Attendees.Count));
            Assert.That(sut.EventGuests, Is.EqualTo(source.EventGuests));
            Assert.That(sut.StaticMapImageLink, Is.EqualTo(MapLinkGenerator.GetStaticImagePreviewLink(sut.LocationDetails!.Value, sut.GoogleMapsApiKey, sut.GoogleMapsPrivateKey)));
            Assert.That(sut.FullMapLink, Is.EqualTo(MapLinkGenerator.GetLinkToFullMap(sut.LocationDetails!.Value.Latitude!.Value, sut.LocationDetails!.Value.Longitude!.Value)));
        });
    }
}
