using FluentAssertions;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Extensions;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.OuterApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Aan.SharedUi.UnitTests.Models.CalendarEvents;

public class NetworkEventDetailsViewModelTests
{
    [Test, RecursiveMoqAutoData]
    public void Constructor_ReceivesOnlineEvent_SetsAllPropertiesExceptLocationDetails(CalendarEvent source)
    {
        source.EventFormat = EventFormat.Online;

        var sut = new NetworkEventDetailsViewModel(source, Guid.NewGuid());

        Assert.Multiple(() =>
        {
            Assert.That(sut.CalendarEventId, Is.EqualTo(source.CalendarEventId));
            Assert.That(sut.CalendarName, Is.EqualTo(source.CalendarName));
            Assert.That(sut.EventFormat, Is.EqualTo(source.EventFormat));
            Assert.That(sut.StartDate, Is.EqualTo(source.StartDate.UtcToLocalTime().ToString("dddd, d MMMM yyyy")));
            Assert.That(sut.EndDate, Is.EqualTo(source.EndDate.UtcToLocalTime().ToString("dddd, d MMMM yyyy")));
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
            Assert.That(sut.StartDateTime, Is.EqualTo(source.StartDate));
            if (sut.StartDateTime < DateTime.UtcNow)
            {
                Assert.That(sut.IsPastEvent, Is.True);
            }
            else
            {
                Assert.That(sut.IsPastEvent, Is.False);

            }
        });
    }

    [Test]
    [MoqInlineAutoData(1, false)]
    [MoqInlineAutoData(-1, true)]
    [MoqInlineAutoData(-61, true)]
    public void Constructor_SetsIsPastEventFlag(int minutes, bool isInPast, CalendarEvent source)
    {
        source.StartDate = DateTime.UtcNow.AddMinutes(minutes);

        var sut = new NetworkEventDetailsViewModel(source, Guid.NewGuid());

        sut.IsPastEvent.Should().Be(isInPast);
    }

    [Test, RecursiveMoqAutoData]
    public void Constructor_ReceivesHybridEvent_SetsAllProperties(CalendarEvent source)
    {
        source.EventFormat = EventFormat.Hybrid;

        var sut = new NetworkEventDetailsViewModel(source, Guid.NewGuid());

        DoAssertsForInPersonAndHybridEvents(source, sut);
    }

    [Test, RecursiveMoqAutoData]
    public void Constructor_ReceivesInPersonEvent_SetsAllProperties(CalendarEvent source)
    {
        source.EventFormat = EventFormat.InPerson;

        var sut = new NetworkEventDetailsViewModel(source, Guid.NewGuid());

        DoAssertsForInPersonAndHybridEvents(source, sut);
    }

    [Test]
    public void AttendeesContainsCurrentMemberId_IsSignedUpIsTrue()
    {
        var memberId = Guid.NewGuid();
        var source = new CalendarEvent();
        source.Attendees.Add(new Attendee() { MemberId = memberId });

        var sut = new NetworkEventDetailsViewModel(source, memberId);

        Assert.That(sut.IsSignedUp, Is.True);
    }

    [Test]
    public void AttendeesDoesNotContainCurrentMemberId_IsSignedUpIsFalse()
    {
        var memberId = Guid.NewGuid();
        var source = new CalendarEvent();

        var sut = new NetworkEventDetailsViewModel(source, memberId);

        Assert.That(sut.IsSignedUp, Is.False);
    }

    [Test]
    public void GetPartialViewName_EventFormatIsOnline_RetrievesOnlinePartialView()
    {
        var source = new CalendarEvent() { EventFormat = EventFormat.Online };
        var sut = new NetworkEventDetailsViewModel(source, Guid.NewGuid());

        Assert.That(sut.PartialViewName, Is.EqualTo("_OnlineEventPartial.cshtml"));
    }

    [Test]
    public void GetPartialViewName_EventFormatIsInPerson_RetrievesInPersonPartialView()
    {
        var source = new CalendarEvent() { EventFormat = EventFormat.InPerson };
        var sut = new NetworkEventDetailsViewModel(source, Guid.NewGuid());

        Assert.That(sut.PartialViewName, Is.EqualTo("_InPersonEventPartial.cshtml"));
    }

    [Test]
    public void GetPartialViewName_EventFormatIsHybrid_RetrievesInPersonPartialView()
    {
        var source = new CalendarEvent() { EventFormat = EventFormat.Hybrid };
        var sut = new NetworkEventDetailsViewModel(source, Guid.NewGuid());

        Assert.That(sut.PartialViewName, Is.EqualTo("_HybridEventPartial.cshtml"));
    }

    [Test]
    public void GetPartialViewName_EventFormatIsUnknown_Throws()
    {
        var source = new CalendarEvent() { EventFormat = (EventFormat)3 };
        var sut = new NetworkEventDetailsViewModel(source, Guid.NewGuid());

        Assert.That(() => sut.PartialViewName, Throws.InstanceOf<NotImplementedException>());
    }

    [Test, MoqAutoData]
    public void StartTime_ReturnsTimePortionOfStartDate(CalendarEvent calendarEvent)
    {
        var sut = new NetworkEventDetailsViewModel(calendarEvent, Guid.NewGuid());

        Assert.That(sut.StartTime, Is.EqualTo(calendarEvent.StartDate.UtcToLocalTime().ToString("h:mm tt")));
    }

    [Test, MoqAutoData]
    public void EndTime_ReturnsTimePortionOfEndDate(CalendarEvent calendarEvent)
    {
        var sut = new NetworkEventDetailsViewModel(calendarEvent, Guid.NewGuid());

        Assert.That(sut.EndTime, Is.EqualTo(calendarEvent.EndDate.UtcToLocalTime().ToString("h:mm tt")));
    }

    [Test, MoqAutoData]
    public void EmailLink_ReturnsUrlEscapedMailtoLink(CalendarEvent calendarEvent)
    {
        calendarEvent.ContactEmail = "me@email.com";
        calendarEvent.Title = "This Is A Title";

        string expected = $"mailto:{calendarEvent.ContactEmail}?subject=This%20Is%20A%20Title";

        var sut = new NetworkEventDetailsViewModel(calendarEvent, Guid.NewGuid());

        Assert.That(sut.EmailLink, Is.EqualTo(expected));
    }


    [Test, MoqAutoData]
    public void GoogleMapLink_ReturnsExpectedUrl(NetworkEventDetailsViewModel sut)
    {
        Assert.Multiple(() =>
        {
            Assert.That(sut.GoogleMapsLink,
                Is.EqualTo(sut.LocationDetails?.Location == null
                    ? string.Empty
                    : $"https://www.google.com/maps/dir//{sut.LocationDetails?.Location}+{sut.LocationDetails?.Postcode}"));
        });
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
        });
    }
}
