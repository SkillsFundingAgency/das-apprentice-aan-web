using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.Aan.SharedUi.Services;

namespace SFA.DAS.ApprenticeAan.Web.Models.AmbassadorProfile;
public class ApprenticeshipDetailsViewModel
{
    public ApprenticeshipDetailsViewModel(IEnumerable<MemberProfile> memberProfiles, ApprenticeshipDetailsModel? apprenticeshipDetails, IEnumerable<MemberPreference> memberPreferences, IUrlHelper urlHelper)
    {
        var employerName = MapProfilesAndPreferencesService.GetProfileValue(ProfileConstants.ProfileIds.EmployerName, memberProfiles);
        EmployerName = employerName;
        var employerAddress1 = MapProfilesAndPreferencesService.GetProfileValue(ProfileConstants.ProfileIds.EmployerAddress1, memberProfiles);
        var employerAddress2 = MapProfilesAndPreferencesService.GetProfileValue(ProfileConstants.ProfileIds.EmployerAddress2, memberProfiles);
        var employerTown = MapProfilesAndPreferencesService.GetProfileValue(ProfileConstants.ProfileIds.EmployerTownOrCity, memberProfiles);
        var employerCounty = MapProfilesAndPreferencesService.GetProfileValue(ProfileConstants.ProfileIds.EmployerCounty, memberProfiles);
        var employerPostcode = MapProfilesAndPreferencesService.GetProfileValue(ProfileConstants.ProfileIds.EmployerPostcode, memberProfiles);
        var addressArray = new List<string?>() { employerAddress1, employerAddress2, employerTown, employerCounty, employerPostcode };
        EmployerAddress = string.Join($", {Environment.NewLine}", addressArray.Where(x => !string.IsNullOrWhiteSpace(x)));
        ApprenticeshipSector = apprenticeshipDetails?.Sector;
        ApprenticeshipProgramme = apprenticeshipDetails?.Programme;
        ApprenticeshipLevel = apprenticeshipDetails?.Level;
        var (apprenticeshipDetailsDisplayValue, apprenticeshipDetailsDisplayClass) = MapProfilesAndPreferencesService.SetDisplayValue(GetApprenticeshipDetailsPreference(memberPreferences));
        ApprenticeshipDetailsDisplayValue = apprenticeshipDetailsDisplayValue;
        ApprenticeshipDetailsDisplayClass = apprenticeshipDetailsDisplayClass;
        ApprenticeshipInformationChangeUrl = urlHelper.RouteUrl(SharedRouteNames.EditApprenticeshipInformation)!;
    }

    public string? EmployerName { get; set; }
    public string? EmployerAddress { get; set; }
    public string? ApprenticeshipSector { get; set; }
    public string? ApprenticeshipProgramme { get; set; }
    public string? ApprenticeshipLevel { get; set; }
    public string ApprenticeshipDetailsDisplayValue { get; set; }
    public string ApprenticeshipDetailsDisplayClass { get; set; }
    public string ApprenticeshipInformationChangeUrl { get; set; }
    private static bool GetApprenticeshipDetailsPreference(IEnumerable<MemberPreference> memberPreferences)
    {
        return memberPreferences.FirstOrDefault(x => x.PreferenceId == PreferenceConstants.PreferenceIds.Apprenticeship)?.Value ?? false;
    }
}