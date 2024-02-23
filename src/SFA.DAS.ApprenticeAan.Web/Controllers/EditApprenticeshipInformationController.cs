using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models.EditApprenticeshipInformation;
using SFA.DAS.Aan.SharedUi.Services;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using static SFA.DAS.Aan.SharedUi.Constants.PreferenceConstants;
using static SFA.DAS.Aan.SharedUi.Constants.ProfileConstants;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("edit-apprenticeship-information", Name = SharedRouteNames.EditApprenticeshipInformation)]
public class EditApprenticeshipInformationController : Controller
{
    private readonly IOuterApiClient _apiClient;
    private readonly ISessionService _sessionService;
    private readonly IValidator<SubmitApprenticeshipInformationModel> _validator;
    public const string ChangeApprenticeshipInformationViewPath = "~/Views/EditApprenticeshipInformation/EditApprenticeshipInformation.cshtml";

    public EditApprenticeshipInformationController(IOuterApiClient apiClient, IValidator<SubmitApprenticeshipInformationModel> validator, ISessionService sessionService)
    {
        _apiClient = apiClient;
        _validator = validator;
        _sessionService = sessionService;
    }

    [HttpGet]
    public IActionResult Index(CancellationToken cancellationToken)
    {
        EditApprenticeshipInformationViewModel editApprenticeshipInformationViewModel = GetEditApprenticeshipInformationViewModel(cancellationToken).Result;
        return View(ChangeApprenticeshipInformationViewPath, editApprenticeshipInformationViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Post(SubmitApprenticeshipInformationModel submitApprenticeshipInformationModel, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(submitApprenticeshipInformationModel, cancellationToken);
        if (!result.IsValid)
        {
            EditApprenticeshipInformationViewModel editApprenticeshipInformationViewModel = GetEditApprenticeshipInformationViewModel(cancellationToken).Result;
            result.AddToModelState(ModelState);
            return View(ChangeApprenticeshipInformationViewPath, editApprenticeshipInformationViewModel);
        }

        UpdateMemberProfileAndPreferencesRequest updateMemberProfileAndPreferencesRequest = new();

        updateMemberProfileAndPreferencesRequest.PatchMemberRequest.OrganisationName = submitApprenticeshipInformationModel.EmployerName?.Trim();

        List<UpdatePreferenceModel> updatePreferenceModels =
        [
            new UpdatePreferenceModel() { PreferenceId = PreferenceConstants.PreferenceIds.Apprenticeship, Value = submitApprenticeshipInformationModel.ShowApprenticeshipInformation },
        ];
        updateMemberProfileAndPreferencesRequest.UpdateMemberProfileRequest.MemberPreferences = updatePreferenceModels;

        List<UpdateProfileModel> updateProfileModels =
        [
            new UpdateProfileModel() { MemberProfileId = ProfileIds.EmployerName, Value = submitApprenticeshipInformationModel.EmployerName?.Trim() },
            new UpdateProfileModel() { MemberProfileId = ProfileIds.EmployerAddress1, Value = submitApprenticeshipInformationModel.EmployerAddress1?.Trim() },
            new UpdateProfileModel() { MemberProfileId = ProfileIds.EmployerAddress2, Value = submitApprenticeshipInformationModel.EmployerAddress2?.Trim() },
            new UpdateProfileModel() { MemberProfileId = ProfileIds.EmployerCounty, Value = submitApprenticeshipInformationModel.EmployerCounty?.Trim() },
            new UpdateProfileModel() { MemberProfileId = ProfileIds.EmployerTownOrCity, Value = submitApprenticeshipInformationModel.EmployerTownOrCity?.Trim() },
            new UpdateProfileModel() { MemberProfileId = ProfileIds.EmployerPostcode, Value = submitApprenticeshipInformationModel.EmployerPostcode?.Trim() },
        ];
        updateMemberProfileAndPreferencesRequest.UpdateMemberProfileRequest.MemberProfiles = updateProfileModels;

        await _apiClient.UpdateMemberProfileAndPreferences(_sessionService.GetMemberId(), updateMemberProfileAndPreferencesRequest, cancellationToken);

        TempData[TempDataKeys.YourAmbassadorProfileSuccessMessage] = true;
        return RedirectToRoute(SharedRouteNames.YourAmbassadorProfile);
    }

    public async Task<EditApprenticeshipInformationViewModel> GetEditApprenticeshipInformationViewModel(CancellationToken cancellationToken)
    {
        var memberProfiles = await _apiClient.GetMemberProfile(_sessionService.GetMemberId(), _sessionService.GetMemberId(), false, cancellationToken);
        EditApprenticeshipInformationViewModel editApprenticeshipInformationViewModel = new()
        {
            EmployerName = MapProfilesAndPreferencesService.GetProfileValue(ProfileIds.EmployerName, memberProfiles.Profiles),
            EmployerAddress1 = MapProfilesAndPreferencesService.GetProfileValue(ProfileIds.EmployerAddress1, memberProfiles.Profiles),
            EmployerAddress2 = MapProfilesAndPreferencesService.GetProfileValue(ProfileIds.EmployerAddress2, memberProfiles.Profiles),
            EmployerTownOrCity = MapProfilesAndPreferencesService.GetProfileValue(ProfileIds.EmployerTownOrCity, memberProfiles.Profiles),
            EmployerCounty = MapProfilesAndPreferencesService.GetProfileValue(ProfileIds.EmployerCounty, memberProfiles.Profiles),
            EmployerPostcode = MapProfilesAndPreferencesService.GetProfileValue(ProfileIds.EmployerPostcode, memberProfiles.Profiles),

            ShowApprenticeshipInformation = MapProfilesAndPreferencesService.GetPreferenceValue(PreferenceIds.Apprenticeship, memberProfiles.Preferences)
        };

        if (memberProfiles.Apprenticeship != null)
        {
            editApprenticeshipInformationViewModel.Programmes = memberProfiles.Apprenticeship.Programme;
            editApprenticeshipInformationViewModel.Sector = memberProfiles.Apprenticeship.Sector;
            editApprenticeshipInformationViewModel.Level = memberProfiles.Apprenticeship.Level;
        }
        editApprenticeshipInformationViewModel.YourAmbassadorProfileUrl = Url.RouteUrl(SharedRouteNames.YourAmbassadorProfile)!;
        return editApprenticeshipInformationViewModel;
    }
}
