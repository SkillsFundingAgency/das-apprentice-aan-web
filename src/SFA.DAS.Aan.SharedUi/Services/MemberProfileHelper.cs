using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.Aan.SharedUi.Models.PublicProfile;

namespace SFA.DAS.Aan.SharedUi.Services;

public static class MemberProfileHelper
{
    public const string ApprenticeMemberAreasOfInterestTitle = "Here are the areas I’m most interested in as an ambassador.";
    public const string EmployerMemberAreasOfInterestTitle = "Here are my reasons for becoming an ambassador, what support I need and how I can help other members.";
    public const string EmployerMemberApprenticeshipSectionTitle = "Employer information";
    public const string ApprenticeMemberApprenticeshipSectionTitle = "{0}’s apprenticeship information";

    public static string GetLinkedInUrl(List<Profile> profiles, IEnumerable<MemberProfile> memberProfiles)
    {
        var linkedInHandle = GetProfileValueByDescription(MemberProfileConstants.MemberProfileDescription.LinkedIn, profiles, memberProfiles);
        return string.IsNullOrEmpty(linkedInHandle) ? string.Empty : string.Concat(UrlConstant.LinkedinUrl, linkedInHandle);
    }

    public static string? GetProfileValueByDescription(string profileDescription, List<Profile> profiles, IEnumerable<MemberProfile> memberProfiles)
    {
        var profileId = profiles.SingleOrDefault(p => p.Description.Equals(profileDescription, StringComparison.OrdinalIgnoreCase))?.Id;
        if (profileId == null) return null;
        return memberProfiles.FirstOrDefault(m => m.ProfileId == profileId)?.Value;
    }

    public static string? GetEmployerAddress(IEnumerable<MemberProfile> memberProfiles)
    {
        var employerAddressProfileIds = new int[] { 31, 32, 33, 34, 35 };
        var addressProfiles = memberProfiles.Where(m => employerAddressProfileIds.Contains(m.ProfileId) && !string.IsNullOrWhiteSpace(m.Value)).Select(m => m.Value);
        return addressProfiles.Any() ? string.Join(", ", addressProfiles) : null;
    }

    public static AreasOfInterestSectionViewModel CreateAreasOfInterestViewModel(MemberUserType memberUserType, List<Profile> profiles, IEnumerable<MemberProfile> memberProfiles, string firstName)
    {
        IEnumerable<string> getSelectedInterests(string category) =>
            from profile in profiles
            join memberProfile in memberProfiles on profile.Id equals memberProfile.ProfileId
            where profile.Category == category
            orderby profile.Ordering
            select profile.Description;

        if (memberUserType == MemberUserType.Apprentice)
        {
            return new AreasOfInterestSectionViewModel()
            {
                FirstName = firstName,
                SubText = ApprenticeMemberAreasOfInterestTitle,
                Sections =
                [
                    new AreasOfInterestSection(MemberProfileConstants.AreasOfInterest.ApprenticeAreasOfInterestFirstSection.Title, getSelectedInterests(MemberProfileConstants.AreasOfInterest.ApprenticeAreasOfInterestFirstSection.Category)),
                    new AreasOfInterestSection(MemberProfileConstants.AreasOfInterest.ApprenticeAreasOfInterestSecondSection.Title, getSelectedInterests(MemberProfileConstants.AreasOfInterest.ApprenticeAreasOfInterestSecondSection.Category)),
                ]
            };
        }
        else
        {
            return new AreasOfInterestSectionViewModel()
            {
                FirstName = firstName,
                SubText = EmployerMemberAreasOfInterestTitle,
                Sections =
                [
                    new AreasOfInterestSection(MemberProfileConstants.AreasOfInterest.EmployerAreasOfInterestFirstSection.Title, getSelectedInterests(MemberProfileConstants.AreasOfInterest.EmployerAreasOfInterestFirstSection.Category)),
                    new AreasOfInterestSection(MemberProfileConstants.AreasOfInterest.EmployerAreasOfInterestSecondSection.Title, getSelectedInterests(MemberProfileConstants.AreasOfInterest.EmployerAreasOfInterestSecondSection.Category)),
                ]
            };
        }
    }

    public static bool IsApprenticeshipInformationShared(IEnumerable<MemberPreference> preferences) => preferences.Single(p => p.PreferenceId == PreferenceConstants.PreferenceIds.Apprenticeship).Value;

    public static Role ConvertToRole(this MemberUserType memberUserType, bool isRegionalChair)
        => memberUserType switch
        {
            MemberUserType.Apprentice when !isRegionalChair => Role.Apprentice,
            MemberUserType.Employer when !isRegionalChair => Role.Employer,
            _ => Role.RegionalChair
        };

    public static string GetApprenticeshipSectionTitle(MemberUserType memberUserType, string firstName) => memberUserType == MemberUserType.Apprentice ? string.Format(ApprenticeMemberApprenticeshipSectionTitle, firstName) : EmployerMemberApprenticeshipSectionTitle;
}
