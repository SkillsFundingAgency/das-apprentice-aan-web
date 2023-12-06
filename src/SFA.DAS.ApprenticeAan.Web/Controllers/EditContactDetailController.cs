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
public class EditContactDetailController : Controller
{
    private readonly IOuterApiClient _apiClient;
    private readonly IValidator<SubmitContactDetailModel> _validator;
    public const string ChangeContactDetailViewPath = "~/Views/EditContactDetail/EditContactDetail.cshtml";

    public EditContactDetailController(IOuterApiClient apiClient, IValidator<SubmitContactDetailModel> validator)
    {
        _apiClient = apiClient;
        _validator = validator;
    }

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

        UpdateMemberProfileAndPreferencesRequest updateMemberProfileAndPreferencesRequest = new UpdateMemberProfileAndPreferencesRequest();

        List<UpdatePreferenceModel> updatePreferenceModels = new List<UpdatePreferenceModel>();
        updatePreferenceModels.Add(new UpdatePreferenceModel() { PreferenceId = PreferenceConstants.PreferenceIds.LinkedIn, Value = submitContactDetailModel.ShowLinkedinUrl });

        updateMemberProfileAndPreferencesRequest.updateMemberProfileRequest.MemberPreferences = updatePreferenceModels;

        List<UpdateProfileModel> updateProfileModels = new List<UpdateProfileModel>();
        updateProfileModels.Add(new UpdateProfileModel() { MemberProfileId = ProfileIds.LinkedIn, Value = StringFormatter.TrimValue(submitContactDetailModel.LinkedinUrl) });

        updateMemberProfileAndPreferencesRequest.updateMemberProfileRequest.MemberProfiles = updateProfileModels;

        await _apiClient.UpdateMemberProfileAndPreferences(User.GetAanMemberId(), updateMemberProfileAndPreferencesRequest, cancellationToken);

        TempData[TempDataKeys.YourAmbassadorProfileSuccessMessage] = true;
        return RedirectToRoute(SharedRouteNames.YourAmbassadorProfile);
    }

    public async Task<EditContactDetailViewModel> GetContactDetailViewModel(CancellationToken cancellationToken)
    {
        var memberProfile = await _apiClient.GetMemberProfile(User.GetAanMemberId(), User.GetAanMemberId(), false, cancellationToken);

        EditContactDetailViewModel editContactDetailViewModel = new EditContactDetailViewModel();
        editContactDetailViewModel.Email = memberProfile.Email;
        editContactDetailViewModel.LinkedinUrl = MapProfilesAndPreferencesService.GetProfileValue(ProfileIds.LinkedIn, memberProfile.Profiles);
        editContactDetailViewModel.ShowLinkedinUrl = MapProfilesAndPreferencesService.GetPreferenceValue(PreferenceIds.LinkedIn, memberProfile.Preferences);
        editContactDetailViewModel.YourAmbassadorProfileUrl = Url.RouteUrl(SharedRouteNames.YourAmbassadorProfile)!;
        return editContactDetailViewModel;
    }
}
