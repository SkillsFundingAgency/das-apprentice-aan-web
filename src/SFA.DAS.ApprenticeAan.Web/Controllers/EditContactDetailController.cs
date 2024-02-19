using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models.EditContactDetail;
using SFA.DAS.Aan.SharedUi.Services;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using static SFA.DAS.Aan.SharedUi.Constants.PreferenceConstants;
using static SFA.DAS.Aan.SharedUi.Constants.ProfileConstants;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("edit-contact-detail", Name = SharedRouteNames.EditContactDetail)]
public class EditContactDetailController(IOuterApiClient apiClient, IValidator<SubmitContactDetailModel> validator, ISessionService sessionService) : Controller
{
    private readonly IOuterApiClient _apiClient = apiClient;
    private readonly ISessionService _sessionService = sessionService;
    private readonly IValidator<SubmitContactDetailModel> _validator = validator;
    public const string ChangeContactDetailViewPath = "~/Views/EditContactDetail/EditContactDetail.cshtml";

    [HttpGet]
    public IActionResult Index(CancellationToken cancellationToken)
    {
        return View(ChangeContactDetailViewPath, GetContactDetailViewModel(cancellationToken).Result);
    }

    [HttpPost]
    public async Task<IActionResult> Post(SubmitContactDetailModel submitContactDetailModel, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(submitContactDetailModel, cancellationToken);

        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return View(ChangeContactDetailViewPath, GetContactDetailViewModel(cancellationToken).Result);
        }

        UpdateMemberProfileAndPreferencesRequest updateMemberProfileAndPreferencesRequest = new();

        List<UpdatePreferenceModel> updatePreferenceModels =
        [
            new UpdatePreferenceModel() { PreferenceId = PreferenceConstants.PreferenceIds.LinkedIn, Value = submitContactDetailModel.ShowLinkedinUrl },
        ];

        updateMemberProfileAndPreferencesRequest.UpdateMemberProfileRequest.MemberPreferences = updatePreferenceModels;

        List<UpdateProfileModel> updateProfileModels =
        [
            new() { MemberProfileId = ProfileIds.LinkedIn, Value = submitContactDetailModel.LinkedinUrl?.Trim() },
        ];

        updateMemberProfileAndPreferencesRequest.UpdateMemberProfileRequest.MemberProfiles = updateProfileModels;

        await _apiClient.UpdateMemberProfileAndPreferences(_sessionService.GetMemberId(), updateMemberProfileAndPreferencesRequest, cancellationToken);

        TempData[TempDataKeys.YourAmbassadorProfileSuccessMessage] = true;
        return RedirectToRoute(SharedRouteNames.YourAmbassadorProfile);
    }

    public async Task<EditContactDetailViewModel> GetContactDetailViewModel(CancellationToken cancellationToken)
    {
        var memberProfile = await _apiClient.GetMemberProfile(_sessionService.GetMemberId(), _sessionService.GetMemberId(), false, cancellationToken);

        EditContactDetailViewModel editContactDetailViewModel = new()
        {
            Email = memberProfile.Email,
            LinkedinUrl = MapProfilesAndPreferencesService.GetProfileValue(ProfileIds.LinkedIn, memberProfile.Profiles),
            ShowLinkedinUrl = MapProfilesAndPreferencesService.GetPreferenceValue(PreferenceIds.LinkedIn, memberProfile.Preferences),
            YourAmbassadorProfileUrl = Url.RouteUrl(SharedRouteNames.YourAmbassadorProfile)!
        };
        return editContactDetailViewModel;
    }
}
