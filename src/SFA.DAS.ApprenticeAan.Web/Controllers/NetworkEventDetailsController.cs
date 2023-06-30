using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("network-event-details")]
public class NetworkEventDetailsController : Controller
{
    public const string DetailsViewPath = "~/Views/NetworkEventDetails/Detail.cshtml";
    public const string SignUpConfirmationViewPath = "~/Views/NetworkEventDetails/SignUpConfirmation.cshtml";
    public const string CancellationConfirmationViewPath = "~/Views/NetworkEventDetails/CancellationConfirmation.cshtml";

    private readonly IOuterApiClient _outerApiClient;
    private readonly ApplicationConfiguration _applicationConfiguration;
    private readonly IValidator<SubmitAttendanceCommand> _validator;

    public NetworkEventDetailsController(IOuterApiClient outerApiClient, ApplicationConfiguration applicationConfiguration, IValidator<SubmitAttendanceCommand> validator)
    {
        _outerApiClient = outerApiClient;
        _applicationConfiguration = applicationConfiguration;
        _validator = validator;
    }

    [HttpGet]
    [Route("{id}", Name = RouteNames.NetworkEventDetails)]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var memberId = User.GetAanMemberId();
        var eventDetailsResponse = await _outerApiClient.GetCalendarEventDetails(id, memberId, cancellationToken);

        if (eventDetailsResponse.ResponseMessage.IsSuccessStatusCode)
        {
            return View(DetailsViewPath, new NetworkEventDetailsViewModel(
                eventDetailsResponse.GetContent(),
                memberId,
                _applicationConfiguration.ApplicationSettings.GoogleMapsApiKey,
                _applicationConfiguration.ApplicationSettings.GoogleMapsPrivateKey));
        }

        throw new InvalidOperationException($"An event with ID {id} was not found."); //TODO: Navigate to 404
    }

    [HttpPost]
    public async Task<IActionResult> Post(SubmitAttendanceCommand command, CancellationToken cancellationToken)
    {
        var memberId = User.GetAanMemberId();
        var result = _validator.Validate(command);

        if (!result.IsValid)
        {
            var eventDetailsResponse = await _outerApiClient.GetCalendarEventDetails(command.CalendarEventId, memberId, cancellationToken);
            var model = new NetworkEventDetailsViewModel(
                eventDetailsResponse.GetContent(),
                memberId,
                _applicationConfiguration.ApplicationSettings.GoogleMapsApiKey,
                _applicationConfiguration.ApplicationSettings.GoogleMapsPrivateKey);

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
        return View(SignUpConfirmationViewPath);
    }

    [HttpGet]
    [Route("cancellation-confirmation", Name = RouteNames.AttendanceConfirmations.CancellationConfirmation)]
    public IActionResult CancellationConfirmation()
    {
        return View(CancellationConfirmationViewPath);
    }
}
