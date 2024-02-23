using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.Aan.SharedUi.Services;

namespace SFA.DAS.ApprenticeAan.Web.Models.AmbassadorProfile;
public class InterestInTheNetworkViewModel
{
    public InterestInTheNetworkViewModel(IEnumerable<MemberProfile> memberProfiles, List<Profile> profiles, string areaOfInterestChangeUrl)
    {
        var interests = GetInterests(memberProfiles, profiles);
        EventsActivities = SetInterests(ProfileConstants.ProfileCategory.Events, interests);
        PromotionActivities = SetInterests(ProfileConstants.ProfileCategory.Promotions, interests);
        var (displayValue, displayClass) = MapProfilesAndPreferencesService.SetDisplayValue(true);
        InterestInTheNetworkDisplayed = displayValue;
        InterestInTheNetworkDisplayClass = displayClass;
        AreaOfInterestChangeUrl = areaOfInterestChangeUrl;
    }
    public IEnumerable<string> EventsActivities { get; set; }
    public IEnumerable<string> PromotionActivities { get; set; }
    public string InterestInTheNetworkDisplayed { get; set; }
    public string InterestInTheNetworkDisplayClass { get; set; }
    public string AreaOfInterestChangeUrl { get; set; }
    private static Dictionary<string, IEnumerable<ProfileValue>> GetInterests(IEnumerable<MemberProfile> memberProfiles, List<Profile> profiles)
    {
        var categories = profiles.Select(x => x.Category).Distinct().ToArray();
        Dictionary<string, IEnumerable<ProfileValue>> result = [];
        foreach (var category in categories)
        {
            var profileValues = profiles.Where(p => p.Category == category).Select(x => new ProfileValue(x.Description, memberProfiles.Where(m => m.ProfileId == x.Id).Select(m => m.Value).FirstOrDefault()));
            result.Add(category, profileValues);
        }
        return result;
    }

    private static List<string> SetInterests(string category, Dictionary<string, IEnumerable<ProfileValue>> interests) => interests[category]
        .Where(x => x.IsSelected?.ToLower() == "true")
        .Select(x => x.Description)
        .ToList();
}