﻿using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.ApprenticeAan.Web.Models.AmbassadorProfile;

namespace SFA.DAS.Aan.SharedUi.Models;
public class AmbassadorProfileViewModel
{
    public AmbassadorProfileViewModel(PersonalDetailsModel personalDetails, string email, IEnumerable<MemberProfile> memberProfiles, IEnumerable<MemberPreference> memberPreferences, ApprenticeshipDetailsModel? apprenticeshipDetails, List<Profile> profiles, IUrlHelper urlHelper)
    {
        PersonalDetails = new PersonalDetailsViewModel(personalDetails, memberProfiles, memberPreferences);
        InterestInTheNetwork = new InterestInTheNetworkViewModel(memberProfiles, profiles, personalDetails.AreaOfInterestChangeUrl);
        ApprenticeshipDetails = new ApprenticeshipDetailsViewModel(memberProfiles, apprenticeshipDetails, memberPreferences, urlHelper);
        ContactDetails = new ContactDetailsViewModel(email, memberProfiles, memberPreferences, personalDetails.ContactDetailChangeUrl);
        ShowApprenticeshipDetails = GetShowApprenticeshipDetails(ApprenticeshipDetails.EmployerName, ApprenticeshipDetails.EmployerAddress, apprenticeshipDetails);
        MemberProfileUrl = personalDetails.MemberProfileUrl;
    }
    public PersonalDetailsViewModel PersonalDetails { get; set; }
    public InterestInTheNetworkViewModel InterestInTheNetwork { get; set; }
    public ApprenticeshipDetailsViewModel ApprenticeshipDetails { get; set; }
    public ContactDetailsViewModel ContactDetails { get; set; }
    public bool ShowApprenticeshipDetails { get; set; }
    public string MemberProfileUrl { get; set; }
    private static bool GetShowApprenticeshipDetails(string? employerName, string? employerAddress, ApprenticeshipDetailsModel? apprenticeshipDetails)
    {
        if (string.IsNullOrEmpty(employerName) && string.IsNullOrEmpty(employerAddress) && (apprenticeshipDetails == null))
        {
            return false;
        }
        return true;
    }
}