using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Models.CalendarEvents;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models.CalendarEvents;

public class EventDetailsViewModelTests
{
    [Test, RecursiveMoqAutoData]
    public void Constructor_ReceivesOnlineEvent_SetsAllPropertiesExceptLocationDetails(CalendarEvent source)
    {
        source.EventFormat = EventFormat.Online;

        var sut = new EventDetailsViewModel(source, Guid.NewGuid());

        Assert.Multiple(() =>
        {
            Assert.That(sut.CalendarEventId, Is.EqualTo(source.CalendarEventId));
            Assert.That(sut.CalendarName, Is.EqualTo(source.CalendarName));
            Assert.That(sut.EventFormat, Is.EqualTo(source.EventFormat));
            Assert.That(sut.StartDate, Is.EqualTo(source.StartDate));
            Assert.That(sut.EndDate, Is.EqualTo(source.EndDate));
            Assert.That(sut.Description, Is.EqualTo(source.Description));
            Assert.That(sut.Summary, Is.EqualTo(source.Summary));
            Assert.That(sut.LocationDetails.Location, Is.Null);
            Assert.That(sut.LocationDetails.Postcode, Is.Null);
            Assert.That(sut.LocationDetails.Longitude, Is.Null);
            Assert.That(sut.LocationDetails.Latitude, Is.Null);
            Assert.That(sut.LocationDetails.Distance, Is.Null);
            Assert.That(sut.EventLink, Is.EqualTo(source.EventLink));
            Assert.That(sut.ContactName, Is.EqualTo(source.ContactName));
            Assert.That(sut.ContactEmail, Is.EqualTo(source.ContactEmail));
            Assert.That(sut.CancelReason, Is.EqualTo(source.CancelReason));
            Assert.That(sut.Attendees, Is.EqualTo(source.Attendees));
            Assert.That(sut.EventGuests, Is.EqualTo(source.EventGuests));
        });
    }

    [Test, RecursiveMoqAutoData]
    public void Constructor_ReceivesHybridEvent_SetsAllProperties(CalendarEvent source)
    {
        source.EventFormat = EventFormat.Hybrid;

        var sut = new EventDetailsViewModel(source, Guid.NewGuid());

        Assert.Multiple(() =>
        {
            Assert.That(sut.CalendarEventId, Is.EqualTo(source.CalendarEventId));
            Assert.That(sut.CalendarName, Is.EqualTo(source.CalendarName));
            Assert.That(sut.EventFormat, Is.EqualTo(source.EventFormat));
            Assert.That(sut.StartDate, Is.EqualTo(source.StartDate));
            Assert.That(sut.EndDate, Is.EqualTo(source.EndDate));
            Assert.That(sut.Description, Is.EqualTo(source.Description));
            Assert.That(sut.Summary, Is.EqualTo(source.Summary));
            Assert.That(sut.LocationDetails.Location, Is.EqualTo(source.Location));
            Assert.That(sut.LocationDetails.Postcode, Is.EqualTo(source.Postcode));
            Assert.That(sut.LocationDetails.Longitude, Is.EqualTo(source.Longitude));
            Assert.That(sut.LocationDetails.Latitude, Is.EqualTo(source.Latitude));
            Assert.That(sut.LocationDetails.Distance, Is.EqualTo(source.Distance));
            Assert.That(sut.EventLink, Is.EqualTo(source.EventLink));
            Assert.That(sut.ContactName, Is.EqualTo(source.ContactName));
            Assert.That(sut.ContactEmail, Is.EqualTo(source.ContactEmail));
            Assert.That(sut.CancelReason, Is.EqualTo(source.CancelReason));
            Assert.That(sut.Attendees, Is.EqualTo(source.Attendees));
            Assert.That(sut.EventGuests, Is.EqualTo(source.EventGuests));
        });
    }

    [Test, RecursiveMoqAutoData]
    public void Constructor_ReceivesInPersonEvent_SetsAllProperties(CalendarEvent source)
    {
        source.EventFormat = EventFormat.InPerson;

        var sut = new EventDetailsViewModel(source, Guid.NewGuid());

        Assert.Multiple(() =>
        {
            Assert.That(sut.CalendarEventId, Is.EqualTo(source.CalendarEventId));
            Assert.That(sut.CalendarName, Is.EqualTo(source.CalendarName));
            Assert.That(sut.EventFormat, Is.EqualTo(source.EventFormat));
            Assert.That(sut.StartDate, Is.EqualTo(source.StartDate));
            Assert.That(sut.EndDate, Is.EqualTo(source.EndDate));
            Assert.That(sut.Description, Is.EqualTo(source.Description));
            Assert.That(sut.Summary, Is.EqualTo(source.Summary));
            Assert.That(sut.LocationDetails.Location, Is.EqualTo(source.Location));
            Assert.That(sut.LocationDetails.Postcode, Is.EqualTo(source.Postcode));
            Assert.That(sut.LocationDetails.Longitude, Is.EqualTo(source.Longitude));
            Assert.That(sut.LocationDetails.Latitude, Is.EqualTo(source.Latitude));
            Assert.That(sut.LocationDetails.Distance, Is.EqualTo(source.Distance));
            Assert.That(sut.EventLink, Is.EqualTo(source.EventLink));
            Assert.That(sut.ContactName, Is.EqualTo(source.ContactName));
            Assert.That(sut.ContactEmail, Is.EqualTo(source.ContactEmail));
            Assert.That(sut.CancelReason, Is.EqualTo(source.CancelReason));
            Assert.That(sut.Attendees, Is.EqualTo(source.Attendees));
            Assert.That(sut.EventGuests, Is.EqualTo(source.EventGuests));
        });
    }

    [TestCase(EventFormat.Online)]
    [TestCase(EventFormat.InPerson)]
    [TestCase(EventFormat.Hybrid)]
    public void AttendeesContainsCurrentMemberId_IsSignedUpIsTrue(EventFormat eventFormat)
    {
        var memberId = Guid.NewGuid();
        var source = new CalendarEvent() { EventFormat = eventFormat };
        source.Attendees.Add(new Attendee() { MemberId = memberId });

        var sut = new EventDetailsViewModel(source, memberId);

        Assert.That(sut.IsSignedUp, Is.True);
    }

    [TestCase(EventFormat.Online)]
    [TestCase(EventFormat.InPerson)]
    [TestCase(EventFormat.Hybrid)]
    public void AttendeesDoesNotContainCurrentMemberId_IsSignedUpIsFalse(EventFormat eventFormat)
    {
        var memberId = Guid.NewGuid();
        var source = new CalendarEvent() { EventFormat = eventFormat };

        var sut = new EventDetailsViewModel(source, memberId);

        Assert.That(sut.IsSignedUp, Is.False);
    }

    [Test]
    public void GetPartialViewName_EventFormatIsOnline_RetrievesOnlinePartialView()
    {
        var source = new CalendarEvent() { EventFormat = EventFormat.Online };
        var sut = new EventDetailsViewModel(source, Guid.NewGuid());

        Assert.That(sut.PartialViewName, Is.EqualTo("_OnlineEventPartial.cshtml"));
    }

    [Test]
    public void GetPartialViewName_EventFormatIsInPerson_RetrievesInPersonPartialView()
    {
        var source = new CalendarEvent() { EventFormat = EventFormat.InPerson };
        var sut = new EventDetailsViewModel(source, Guid.NewGuid());

        Assert.That(sut.PartialViewName, Is.EqualTo("_InPersonEventPartial.cshtml"));
    }

    [Test]
    public void GetPartialViewName_EventFormatIsHybrid_RetrievesHybridPartialView()
    {
        var source = new CalendarEvent() { EventFormat = EventFormat.Hybrid };
        var sut = new EventDetailsViewModel(source, Guid.NewGuid());

        Assert.That(sut.PartialViewName, Is.EqualTo("_HybridEventPartial.cshtml"));
    }
}
