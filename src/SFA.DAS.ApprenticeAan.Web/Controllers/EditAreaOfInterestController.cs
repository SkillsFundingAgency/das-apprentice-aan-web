using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.Aan.SharedUi.Models.EditAreaOfInterest;
using SFA.DAS.Aan.SharedUi.Services;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("edit-area-of-interest", Name = SharedRouteNames.EditAreaOfInterest)]
public class EditAreaOfInterestController(IValidator<SubmitAreaOfInterestModel> validator, IOuterApiClient outerApiClient, ISessionService sessionService) : Controller
{
    public const string ChangeAreaOfInterestViewPath = "~/Views/EditAreaOfInterest/EditAreaOfInterest.cshtml";

    private readonly IValidator<SubmitAreaOfInterestModel> _validator = validator;
    private readonly ISessionService _sessionService = sessionService;
    private readonly IOuterApiClient _outerApiClient = outerApiClient;

    [HttpGet]
    public IActionResult Get(CancellationToken cancellationToken)
    {
        return View(ChangeAreaOfInterestViewPath, GetAreaOfInterests(cancellationToken).Result);
    }

    [HttpPost]
    public async Task<IActionResult> Post(SubmitAreaOfInterestModel command, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(command, cancellationToken);

        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return View(ChangeAreaOfInterestViewPath, GetAreaOfInterests(cancellationToken).Result);
        }

        UpdateMemberProfileAndPreferencesRequest updateMemberProfileAndPreferencesRequest = new();

        updateMemberProfileAndPreferencesRequest.UpdateMemberProfileRequest.MemberProfiles = command.AreasOfInterest.Select(x => new UpdateProfileModel()
        {
            MemberProfileId = x.Id,
            Value = x.IsSelected ? true.ToString() : null!
        }).ToList();

        await _outerApiClient.UpdateMemberProfileAndPreferences(_sessionService.GetMemberId(), updateMemberProfileAndPreferencesRequest, cancellationToken);
        TempData[TempDataKeys.YourAmbassadorProfileSuccessMessage] = true;
        return RedirectToRoute(SharedRouteNames.YourAmbassadorProfile);
    }

    public async Task<EditAreaOfInterestViewModel> GetAreaOfInterests(CancellationToken cancellationToken)
    {
        EditAreaOfInterestViewModel editAreaOfInterestViewModel = new();
        var memberProfiles = await _outerApiClient.GetMemberProfile(_sessionService.GetMemberId(), _sessionService.GetMemberId(), false, cancellationToken);
        var profilesResult = await _outerApiClient.GetProfilesByUserType(MemberUserType.Apprentice.ToString(), cancellationToken);

        editAreaOfInterestViewModel.FirstSectionInterests = SelectProfileViewModelMapping(profilesResult.Profiles.Where(x => x.Category == Category.Events).ToList(), memberProfiles.Profiles);

        editAreaOfInterestViewModel.SecondSectionInterests = SelectProfileViewModelMapping(profilesResult.Profiles.Where(x => x.Category == Category.Promotions).ToList(), memberProfiles.Profiles);
        editAreaOfInterestViewModel.YourAmbassadorProfileUrl = Url.RouteUrl(SharedRouteNames.YourAmbassadorProfile)!;

        editAreaOfInterestViewModel.FirstSectionTitle = AreaOfInterestTitleConstant.FirstSectionTitleForApprentice;
        editAreaOfInterestViewModel.SecondSectionTitle = AreaOfInterestTitleConstant.SecondSectionTitleForApprentice;
        return editAreaOfInterestViewModel;
    }

    public static List<SelectProfileViewModel> SelectProfileViewModelMapping(IEnumerable<Profile> profiles, IEnumerable<MemberProfile> memberProfiles)
    {
        return
        [
            .. profiles.Select(profile => new SelectProfileViewModel()
            {
                Id = profile.Id,
                Description = profile.Description,
                Category = profile.Category,
                Ordering = profile.Ordering,
                IsSelected = (MapProfilesAndPreferencesService.GetProfileValue(profile.Id, memberProfiles) == true.ToString())
            }).OrderBy(x => x.Ordering),
        ];
    }
}