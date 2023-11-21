using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.Aan.SharedUi.Services;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using static SFA.DAS.Aan.SharedUi.Constants.ProfileConstants;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("edit-personal-information", Name = SharedRouteNames.EditPersonalInformation)]
public class EditPersonalInformationController : Controller
{
    private readonly IOuterApiClient _apiClient;
    private readonly IValidator<SubmitPersonalDetailCommand> _validator;
    public const string ChangePersonalDetailViewPath = "~/Views/EditPersonalInformation/EditPersonalInformation.cshtml";
    public EditPersonalInformationController(IOuterApiClient apiClient, IValidator<SubmitPersonalDetailCommand> validator)
    {
        _apiClient = apiClient;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var memberProfiles = await _apiClient.GetMemberProfile(User.GetAanMemberId(), User.GetAanMemberId(), false, cancellationToken);
        var regionTask = await _apiClient.GetRegions();
        EditPersonalInformationViewModel memberProfile = EditPersonalInformationViewModelMapping(memberProfiles.RegionId ?? 0, memberProfiles.Profiles, memberProfiles.Preferences, memberProfiles.UserType, memberProfiles.OrganisationName);
        memberProfile.Regions = Region.RegionToRegionViewModelMapping(regionTask.Regions);
        memberProfile.YourAmbassadorProfileUrl = Url.RouteUrl(SharedRouteNames.YourAmbassadorProfile)!;
        return View(ChangePersonalDetailViewPath, memberProfile);
    }

    [HttpPost]
    public async Task<IActionResult> Post(SubmitPersonalDetailCommand command, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(command, cancellationToken);

        if (!result.IsValid)
        {
            var memberProfiles = await _apiClient.GetMemberProfile(User.GetAanMemberId(), User.GetAanMemberId(), false, cancellationToken);
            var regionTask = await _apiClient.GetRegions();
            EditPersonalInformationViewModel memberProfile = EditPersonalInformationViewModelMapping(memberProfiles.RegionId ?? 0, memberProfiles.Profiles, memberProfiles.Preferences, memberProfiles.UserType, memberProfiles.OrganisationName);
            memberProfile.Regions = Region.RegionToRegionViewModelMapping(regionTask.Regions);
            memberProfile.YourAmbassadorProfileUrl = Url.RouteUrl(SharedRouteNames.YourAmbassadorProfile)!;
            result.AddToModelState(ModelState);
            return View(ChangePersonalDetailViewPath, memberProfile);
        }
        UpdateMemberProfileAndPreferencesRequest updateMemberProfileAndPreferencesRequest = new UpdateMemberProfileAndPreferencesRequest();
        updateMemberProfileAndPreferencesRequest.patchMemberRequest.RegionId = command.RegionId;
        updateMemberProfileAndPreferencesRequest.patchMemberRequest.OrganisationName = command.OrganisationName;
        List<UpdatePreferenceModel> updatePreferenceModels = new List<UpdatePreferenceModel>();
        updatePreferenceModels.Add(new UpdatePreferenceModel() { Id = PreferenceConstants.PreferenceIds.Biography, Value = command.ShowBiography });
        updatePreferenceModels.Add(new UpdatePreferenceModel() { Id = PreferenceConstants.PreferenceIds.JobTitle, Value = command.ShowJobTitle });

        updateMemberProfileAndPreferencesRequest.updateMemberProfileRequest.Preferences = updatePreferenceModels;

        List<UpdateProfileModel> updateProfileModels = new List<UpdateProfileModel>();
        updateProfileModels.Add(new UpdateProfileModel() { Id = ProfileIds.Biography, Value = command.Biography });
        updateProfileModels.Add(new UpdateProfileModel() { Id = ProfileIds.JobTitle, Value = command.JobTitle });

        updateMemberProfileAndPreferencesRequest.updateMemberProfileRequest.Profiles = updateProfileModels;

        await _apiClient.UpdateMemberProfileAndPreferences(User.GetAanMemberId(), updateMemberProfileAndPreferencesRequest, cancellationToken);
        TempData[TempDataKeys.YourAmbassadorProfileSuccessMessage] = "You have successfully updated your ambassador profile.";
        return RedirectToRoute(SharedRouteNames.YourAmbassadorProfile);
    }

    public static EditPersonalInformationViewModel EditPersonalInformationViewModelMapping(int regionId, IEnumerable<MemberProfile> memberProfiles, IEnumerable<MemberPreference> memberPreferences, MemberUserType userType, string? organisationName)
    {
        EditPersonalInformationViewModel memberProfile = new EditPersonalInformationViewModel();
        memberProfile.RegionId = regionId;
        memberProfile.ShowBiography = MapProfilesAndPreferencesService.GetPreferenceValue(PreferenceConstants.PreferenceIds.Biography, memberPreferences);
        memberProfile.ShowJobTitle = MapProfilesAndPreferencesService.GetPreferenceValue(PreferenceConstants.PreferenceIds.JobTitle, memberPreferences);
        memberProfile.Biography = MapProfilesAndPreferencesService.GetProfileValue(ProfileIds.Biography, memberProfiles);
        memberProfile.JobTitle = MapProfilesAndPreferencesService.GetProfileValue(ProfileIds.JobTitle, memberProfiles);
        memberProfile.UserType = userType;
        memberProfile.OrganisationName = organisationName;

        return memberProfile;
    }
}