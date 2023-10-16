using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("network-events")]
public class NetworkEventDetailsController : Controller
{
    public const string DetailsViewPath = "~/Views/NetworkEventDetails/Detail.cshtml";
    public const string SignUpConfirmationViewPath = "~/Views/NetworkEventDetails/SignUpConfirmation.cshtml";
    public const string CancellationConfirmationViewPath = "~/Views/NetworkEventDetails/CancellationConfirmation.cshtml";

    private readonly IOuterApiClient _outerApiClient;
    private readonly IValidator<SubmitAttendanceCommand> _validator;

    public NetworkEventDetailsController(IOuterApiClient outerApiClient, IValidator<SubmitAttendanceCommand> validator)
    {
        _outerApiClient = outerApiClient;
        _validator = validator;
    }

    [HttpGet]
    [Route("{id}", Name = SharedRouteNames.NetworkEventDetails)]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var memberId = User.GetAanMemberId();
        var eventDetailsResponse = await _outerApiClient.GetCalendarEventDetails(id, memberId, cancellationToken);

        if (eventDetailsResponse.ResponseMessage.IsSuccessStatusCode)
        {
            CalendarEvent calendarEvent = InitialiseViewModel(eventDetailsResponse.GetContent());
            return View(DetailsViewPath, new NetworkEventDetailsViewModel(
                calendarEvent,
                memberId));
        }

        throw new InvalidOperationException($"An event with ID {id} was not found.");
    }

    private CalendarEvent InitialiseViewModel(CalendarEvent result)
    {
        List<Attendee> attendees = new List<Attendee>();
        foreach (var attendee in result.Attendees)
        {
            Attendee attendeeObject = attendee;
            attendeeObject.MemberProfileLink = Url.RouteUrl(SharedRouteNames.MemberProfile, new { id = attendee.MemberId })!;
            attendees.Add(attendeeObject);
        }
        result.Attendees = attendees;
        return result;
    }

    [HttpPost]
    [Route("{id}", Name = SharedRouteNames.NetworkEventDetails)]
    public async Task<IActionResult> Post(SubmitAttendanceCommand command, CancellationToken cancellationToken)
    {
        var memberId = User.GetAanMemberId();
        var result = await _validator.ValidateAsync(command, cancellationToken);

        if (!result.IsValid)
        {
            var eventDetailsResponse = await _outerApiClient.GetCalendarEventDetails(command.CalendarEventId, memberId, cancellationToken);
            var model = new NetworkEventDetailsViewModel(
                eventDetailsResponse.GetContent(),
                memberId);

            result.AddToModelState(ModelState);

            if (eventDetailsResponse.ResponseMessage.IsSuccessStatusCode)
            {
                return View(DetailsViewPath, model);
            }
        }

        await _outerApiClient.PutAttendance(command.CalendarEventId, memberId, new SetAttendanceStatusRequest(command.NewStatus), cancellationToken);

        return command.NewStatus
            ? RedirectToAction("SignUpConfirmation")
            : RedirectToAction("CancellationConfirmation");
    }


    [HttpGet]
    [Route("signup-confirmation", Name = RouteNames.AttendanceConfirmations.SignUpConfirmation)]
    public IActionResult SignUpConfirmation()
    {
        EventAttendanceConfirmationViewModel model = new(Url.RouteUrl(SharedRouteNames.NetworkEvents)!, new(Url.RouteUrl(SharedRouteNames.EventsHub)));
        return View(SignUpConfirmationViewPath, model);
    }

    [HttpGet]
    [Route("cancellation-confirmation", Name = RouteNames.AttendanceConfirmations.CancellationConfirmation)]
    public IActionResult CancellationConfirmation()
    {
        EventAttendanceConfirmationViewModel model = new(Url.RouteUrl(SharedRouteNames.NetworkEvents)!, new(Url.RouteUrl(SharedRouteNames.EventsHub)));
        return View(CancellationConfirmationViewPath, model);
    }
}
