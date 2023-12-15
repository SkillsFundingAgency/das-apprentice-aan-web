using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.Aan.SharedUi.Models.PublicProfile;
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
    public const string ApprenticeMemberAreasOfInterestTitle = "Here are the areas I’m most interested in as an ambassador.";
    public const string EmployerMemberAreasOfInterestTitle = "Here are my reasons for becoming an ambassador, what support I need and how I can help other members.";
    public const string EmployerMemberApprenticeshipSectionTitle = "Employer information";
    public const string ApprenticeMemberApprenticeshipSectionTitle = "{0}’s apprenticeship information";

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
        MemberProfileViewModel model = new();
        var userId = User.GetAanMemberId();
        model.MemberId = id;

        model.PersonalInformation.FullName = memberProfiles.FullName;
        model.PersonalInformation.RegionName = memberProfiles.RegionName;
        model.PersonalInformation.UserRole = memberProfiles.IsRegionalChair ? MemberProfileConstants.RegionalChair : memberProfiles.UserType.ToString();
        model.PersonalInformation.Biography = GetProfileValueByDescription(MemberProfileConstants.MemberProfileDescription.Biography, profilesResult.Profiles, memberProfiles.Profiles);
        model.PersonalInformation.JobTitle = GetProfileValueByDescription(MemberProfileConstants.MemberProfileDescription.JobTitle, profilesResult.Profiles, memberProfiles.Profiles);
        model.LinkedInUrl = GetLinkedInUrl(profilesResult.Profiles, memberProfiles.Profiles);
        model.Email = memberProfiles.Email;
        model.FirstName = memberProfiles.FirstName;

        model.IsLoggedInUserMemberProfile = id == userId;
        model.IsConnectWithMemberVisible = true;

        model.IsEmployerInformationAvailable = memberProfiles.UserType == MemberUserType.Employer && memberProfiles.Preferences.Single(p => p.PreferenceId == PreferenceConstants.PreferenceIds.Apprenticeship).Value;
        model.IsApprenticeshipInformationAvailable = memberProfiles.UserType == MemberUserType.Apprentice && memberProfiles.Preferences.Single(p => p.PreferenceId == PreferenceConstants.PreferenceIds.Apprenticeship).Value;
        if (model.IsEmployerInformationAvailable || model.IsApprenticeshipInformationAvailable)
        {
            model.ApprenticeshipSectionTitle = memberProfiles.UserType == MemberUserType.Apprentice ? string.Format(ApprenticeMemberApprenticeshipSectionTitle, memberProfiles.FirstName) : EmployerMemberApprenticeshipSectionTitle;
            model.EmployerName = memberProfiles.OrganisationName;
            model.EmployerAddress = GetEmployerAddress(memberProfiles.Profiles);

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

        model.AreasOfInterest = CreateAreasOfInterestViewModel(memberProfiles.UserType, profilesResult.Profiles, memberProfiles.Profiles);

        return model;
    }

    private static string? GetEmployerAddress(IEnumerable<MemberProfile> memberProfiles)
    {
        var employerAddressProfileIds = new int[] { 31, 32, 33, 34, 35 };
        var addressProfiles = memberProfiles.Where(m => employerAddressProfileIds.Contains(m.ProfileId) && !string.IsNullOrWhiteSpace(m.Value)).Select(m => m.Value);
        return addressProfiles.Any() ? string.Join(", ", addressProfiles) : null;
    }

    private static string GetLinkedInUrl(List<Profile> profiles, IEnumerable<MemberProfile> memberProfiles)
    {
        var linkedInHandle = GetProfileValueByDescription(MemberProfileConstants.MemberProfileDescription.LinkedIn, profiles, memberProfiles);
        return string.IsNullOrEmpty(linkedInHandle) ? string.Empty : string.Concat(UrlConstant.LinkedinUrl, linkedInHandle);
    }

    private static AreasOfInterestViewModel CreateAreasOfInterestViewModel(MemberUserType memberUserType, List<Profile> profiles, IEnumerable<MemberProfile> memberProfiles)
    {
        if (memberUserType == MemberUserType.Apprentice)
        {
            return new AreasOfInterestViewModel()
            {
                Title = ApprenticeMemberAreasOfInterestTitle,
                Sections = new()
                {
                    new AreasOfInterestSection(MemberProfileConstants.AreasOfInterest.ApprenticeAreasOfInterestFirstSection.Title, GetSelectedInterests(MemberProfileConstants.AreasOfInterest.ApprenticeAreasOfInterestFirstSection.Category, profiles, memberProfiles)),
                    new AreasOfInterestSection(MemberProfileConstants.AreasOfInterest.ApprenticeAreasOfInterestSecondSection.Title, GetSelectedInterests(MemberProfileConstants.AreasOfInterest.ApprenticeAreasOfInterestSecondSection.Category, profiles, memberProfiles)),
                }
            };
        }
        else
        {

            return new AreasOfInterestViewModel()
            {
                Title = EmployerMemberAreasOfInterestTitle,
                Sections = new()
                {
                    new AreasOfInterestSection(MemberProfileConstants.AreasOfInterest.EmployerAreasOfInterestFirstSection.Title, GetSelectedInterests(MemberProfileConstants.AreasOfInterest.EmployerAreasOfInterestFirstSection.Category, profiles, memberProfiles)),
                    new AreasOfInterestSection(MemberProfileConstants.AreasOfInterest.EmployerAreasOfInterestSecondSection.Title, GetSelectedInterests(MemberProfileConstants.AreasOfInterest.EmployerAreasOfInterestFirstSection.Category, profiles, memberProfiles)),
                }
            };
        }
    }

    private static IEnumerable<string> GetSelectedInterests(string category, IEnumerable<Profile> profiles, IEnumerable<MemberProfile> memberProfiles)
    {
        return
            from profile in profiles
            join memberProfile in memberProfiles on profile.Id equals memberProfile.ProfileId
            where profile.Category == category
            orderby profile.Ordering
            select profile.Description;
    }

    private static string? GetProfileValueByDescription(string profileDescription, List<Profile> profiles, IEnumerable<MemberProfile> memberProfiles)
    {
        var profileId = profiles.Single(p => p.Description.Equals(profileDescription, StringComparison.OrdinalIgnoreCase)).Id;
        return memberProfiles.FirstOrDefault(m => m.ProfileId == profileId)?.Value;
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
