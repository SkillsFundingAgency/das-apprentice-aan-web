using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Web.Constant;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

public class CheckYourAnswersViewModel
{
    public string CurrentEmployerName { get; }
    public string CurrentEmployerAddress { get; }
    public string CurrentEmployerChangeLink { get; }
    public string JobTitleChangeLink { get; }
    public string? JobTitle { get; }
    public string RegionChangeLink { get; }
    public string LocationLabel { get; }
    public string ReceiveNotificationsChangeLink { get; }
    public string SelectNotificationEventsChangeLink { get; }
    public string NotificationsLocationsChangeLink { get; }
    public string? Region { get; }
    public List<string>? EventTypes { get; }
    public bool ReceiveNotifications { get; }
    public bool ShowAllEventNotificationQuestions { get; }
    public List<string> NotificationLocations { get; }
    public string ReasonForJoiningTheNetworkChangeLink { get; }
    public string? ReasonForJoiningTheNetwork { get; }
    public string AreasOfInterestChangeLink { get; }
    public List<string>? Events { get; }
    public List<string>? Promotions { get; }
    public string PreviousEngagementChangeLink { get; }
    public string? PreviousEngagement { get; }
    public string FullName { get; }
    public string Email { get; }
    public string? ApprenticeshipDuration { get; }
    public string? ApprenticeshipSector { get; }
    public string? ApprenticeshipProgram { get; }
    public string? ApprenticeshipLevel { get; }

    public CheckYourAnswersViewModel(IUrlHelper url, OnboardingSessionModel sessionModel)
    {
        CurrentEmployerChangeLink = url.RouteUrl(@RouteNames.Onboarding.EmployerSearch)!;
        CurrentEmployerName = sessionModel.GetProfileValue(ProfileConstants.ProfileIds.EmployerName)!;
        CurrentEmployerAddress = GetEmployerAddress(sessionModel)!;

        JobTitleChangeLink = url.RouteUrl(@RouteNames.Onboarding.CurrentJobTitle)!;
        JobTitle = sessionModel.GetProfileValue(ProfileConstants.ProfileIds.JobTitle)!;

        RegionChangeLink = url.RouteUrl(@RouteNames.Onboarding.Regions)!;
        Region = sessionModel.RegionName!;

        ReasonForJoiningTheNetworkChangeLink = url.RouteUrl(@RouteNames.Onboarding.ReasonToJoin)!;
        ReasonForJoiningTheNetwork = sessionModel.GetProfileValue(ProfileConstants.ProfileIds.ReasonToJoinAmbassadorNetwork);

        AreasOfInterestChangeLink = url.RouteUrl(@RouteNames.Onboarding.AreasOfInterest)!;
        Events = sessionModel.ProfileData.Where(x => (x.Category == Category.Events) && x.Value != null).Select(x => x.Description).ToList();
        Promotions = sessionModel.ProfileData.Where(x => (x.Category == Category.Promotions) && x.Value != null).Select(x => x.Description).ToList();

        PreviousEngagementChangeLink = url.RouteUrl(@RouteNames.Onboarding.PreviousEngagement)!;
        PreviousEngagement = GetPreviousEngagementValue(sessionModel.GetProfileValue(ProfileConstants.ProfileIds.EngagedWithAPreviousAmbassadorInTheNetworkApprentice))!;

        /// Apprentice's details
        FullName = sessionModel.ApprenticeDetails.Name;
        Email = sessionModel.ApprenticeDetails.Email;

        /// Apprenticeship details
        var myApprenticeship = sessionModel.MyApprenticeship;
        ApprenticeshipDuration = $"From {myApprenticeship.StartDate.GetValueOrDefault().Date:dd-MM-yyyy} to {myApprenticeship.EndDate.GetValueOrDefault().Date:dd-MM-yyyy}";
        ApprenticeshipSector = myApprenticeship.TrainingCourse?.Sector;
        ApprenticeshipProgram = myApprenticeship.TrainingCourse?.Name;
        ApprenticeshipLevel = myApprenticeship.TrainingCourse?.Level.ToString();

        ReceiveNotificationsChangeLink = url.RouteUrl(@RouteNames.Onboarding.ReceiveNotifications)!;
        ReceiveNotifications = sessionModel.ReceiveNotifications ?? false;

        SelectNotificationEventsChangeLink = url.RouteUrl(@RouteNames.Onboarding.SelectNotificationEvents)!;
        EventTypes = GetEventTypes(sessionModel);

        NotificationsLocationsChangeLink = url.RouteUrl(@RouteNames.Onboarding.NotificationsLocations)!;
        NotificationLocations = GetNotificationLocations(sessionModel);

        LocationLabel = GetLocationLabel(sessionModel);

        ShowAllEventNotificationQuestions = sessionModel.ReceiveNotifications == true
                                            && sessionModel.EventTypes != null
                                            && sessionModel.EventTypes.Any(x => x.IsSelected && x.EventType != EventType.Online);
    }

    private static string GetLocationLabel(OnboardingSessionModel sessionModel)
    {
        if (sessionModel.EventTypes == null || !sessionModel.EventTypes.Any(x => x.IsSelected))
            return string.Empty;

        var selectedTypes = sessionModel.EventTypes.Where(x => x.IsSelected).Select(x => x.EventType).ToList();

        if (selectedTypes.Contains(EventType.All) ||
            (selectedTypes.Contains(EventType.Hybrid) && selectedTypes.Contains(EventType.InPerson)))
            return "in-person and hybrid";

        if (selectedTypes.Contains(EventType.Hybrid))
            return "hybrid";

        if (selectedTypes.Contains(EventType.InPerson))
            return "in-person";

        return string.Empty;
    }

    private static List<string> GetNotificationLocations(OnboardingSessionModel sessionModel)
    {
        return sessionModel.NotificationLocations?
            .Select(x => x.Radius == 0
                ? $"{x.LocationName}, Across England"
                : $"{x.LocationName}, within {x.Radius} miles")
            .ToList() ?? new List<string>();
    }

    private static List<string> GetEventTypes(OnboardingSessionModel sessionModel)
    {
        if (sessionModel.EventTypes == null || !sessionModel.EventTypes.Any())
        {
            return new List<string>();
        }

        bool isAllSelected = sessionModel.EventTypes.Any(x => x.IsSelected && x.EventType == EventType.All);
        return isAllSelected
            ? sessionModel.EventTypes.Where(x => x.EventType != EventType.All).Select(x => x.EventType).ToList()
            : sessionModel.EventTypes.Where(x => x.IsSelected && x.EventType != EventType.All).Select(x => x.EventType).ToList();
    }

    private static string? GetPreviousEngagementValue(string? previousEngagementValue)
    {
        string? resultValue = null;

        if (bool.TryParse(previousEngagementValue, out var result))
            resultValue = result ? "Yes" : "No";

        return resultValue;
    }

    private static string? GetEmployerAddress(OnboardingSessionModel sessionModel)
    {
        var addressFields = new[] { sessionModel.GetProfileValue(ProfileConstants.ProfileIds.EmployerAddress1), sessionModel.GetProfileValue(ProfileConstants.ProfileIds.EmployerAddress2), sessionModel.GetProfileValue(ProfileConstants.ProfileIds.EmployerTownOrCity), sessionModel.GetProfileValue(ProfileConstants.ProfileIds.EmployerCounty), sessionModel.GetProfileValue(ProfileConstants.ProfileIds.EmployerPostcode) };
        string completeAddress = string.Join(", ", addressFields.Where(s => !string.IsNullOrEmpty(s)));
        return string.IsNullOrWhiteSpace(completeAddress) ? null : completeAddress;
    }
}
