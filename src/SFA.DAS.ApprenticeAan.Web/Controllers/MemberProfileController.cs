using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.Aan.SharedUi.Models.PublicProfile;
using SFA.DAS.Aan.SharedUi.Services;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Web.Extensions;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("member-profile")]
public class MemberProfileController : Controller
{
    public const string MemberProfileViewPath = "~/Views/MemberProfile/PublicProfile.cshtml";
    public const string NotificationSentConfirmationViewPath = "~/Views/MemberProfile/NotificationSentConfirmation.cshtml";

    private readonly IOuterApiClient _outerApiClient;

    private readonly IValidator<ConnectWithMemberSubmitModel> _validator;
    public MemberProfileController(IOuterApiClient outerApiClient, IValidator<ConnectWithMemberSubmitModel> validator)
    {
        _outerApiClient = outerApiClient;
        _validator = validator;
    }

    [HttpGet]
    [Route("{id}", Name = SharedRouteNames.MemberProfile)]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        MemberProfileViewModel model = await GetViewModel(id, cancellationToken);

        return View(MemberProfileViewPath, model);
    }

    private async Task<MemberProfileViewModel> GetViewModel(Guid id, CancellationToken cancellationToken)
    {
        var memberProfiles = await _outerApiClient.GetMemberProfile(id, User.GetAanMemberId(), true, cancellationToken);
        var profilesResult = await _outerApiClient.GetProfilesByUserType(memberProfiles.UserType.ToString(), cancellationToken);
        var userId = User.GetAanMemberId();

        MemberProfileViewModel model = new();
        model.MemberId = id;

        model.PersonalInformation.FullName = memberProfiles.FullName;
        model.PersonalInformation.RegionName = memberProfiles.RegionName;
        model.PersonalInformation.UserRole = memberProfiles.UserType.ConvertToRole(memberProfiles.IsRegionalChair);
        model.PersonalInformation.Biography = MemberProfileHelper.GetProfileValueByDescription(MemberProfileConstants.MemberProfileDescription.Biography, profilesResult.Profiles, memberProfiles.Profiles);
        model.PersonalInformation.JobTitle = MemberProfileHelper.GetProfileValueByDescription(MemberProfileConstants.MemberProfileDescription.JobTitle, profilesResult.Profiles, memberProfiles.Profiles);
        model.LinkedInUrl = MemberProfileHelper.GetLinkedInUrl(profilesResult.Profiles, memberProfiles.Profiles);
        model.Email = memberProfiles.Email;
        model.FirstName = memberProfiles.FirstName;

        model.IsLoggedInUserMemberProfile = id == userId;
        model.IsConnectWithMemberVisible = true;

        model.IsEmployerInformationAvailable = memberProfiles.UserType == MemberUserType.Employer && MemberProfileHelper.IsApprenticeshipInformationShared(memberProfiles.Preferences);
        model.IsApprenticeshipInformationAvailable = memberProfiles.UserType == MemberUserType.Apprentice && MemberProfileHelper.IsApprenticeshipInformationShared(memberProfiles.Preferences);

        if (model.IsEmployerInformationAvailable || model.IsApprenticeshipInformationAvailable)
        {
            model.ApprenticeshipSectionTitle = MemberProfileHelper.GetApprenticeshipSectionTitle(memberProfiles.UserType, memberProfiles.FirstName);
            model.EmployerName = memberProfiles.OrganisationName;
            model.EmployerAddress = MemberProfileHelper.GetEmployerAddress(memberProfiles.Profiles);

            if (memberProfiles.Apprenticeship != null)
            {
                //following are only applicable to Apprentice user, the values are assumed to be null otherwise
                model.Sector = memberProfiles.Apprenticeship.Sector;
                model.Programme = memberProfiles.Apprenticeship.Programme;
                model.Level = memberProfiles.Apprenticeship.Level;

                //following are only applicable to Employer user, the values are assumed to be null otherwise
                model.Sectors = memberProfiles.Apprenticeship.Sectors;
                model.ActiveApprenticesCount = memberProfiles.Apprenticeship.ActiveApprenticesCount;
            }
        }

        model.AreasOfInterest = MemberProfileHelper.CreateAreasOfInterestViewModel(memberProfiles.UserType, profilesResult.Profiles, memberProfiles.Profiles);

        return model;
    }

    [HttpPost]
    [Route("{id}", Name = SharedRouteNames.MemberProfile)]
    public async Task<IActionResult> Post([FromRoute] Guid id, ConnectWithMemberSubmitModel command, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(command, cancellationToken);

        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            MemberProfileViewModel model = await GetViewModel(id, cancellationToken);
            return View(MemberProfileViewPath, model);
        }
        CreateNotificationRequest createNotificationRequest = new CreateNotificationRequest(id, command.ReasonToGetInTouch);
        await _outerApiClient.PostNotification(User.GetAanMemberId(), createNotificationRequest, cancellationToken);

        return RedirectToRoute(SharedRouteNames.NotificationSentConfirmation);
    }


    [HttpGet]
    [Route("notification-sent-confirmation", Name = SharedRouteNames.NotificationSentConfirmation)]
    public IActionResult NotificationSentConfirmation()
    {
        NotificationSentConfirmationViewModel model = new(Url.RouteUrl(SharedRouteNames.NetworkDirectory)!);
        return View(NotificationSentConfirmationViewPath, model);
    }
}
