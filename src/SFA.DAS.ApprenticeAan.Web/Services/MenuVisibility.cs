using System.Diagnostics.CodeAnalysis;
using SFA.DAS.ApprenticePortal.SharedUi.Services;

namespace SFA.DAS.ApprenticeAan.Web.Services;
[ExcludeFromCodeCoverage] // TODO Should add coverage after proper implementation post authentication changes

public class MenuVisibility : IMenuVisibility
{
    public Task<ConfirmMyApprenticeshipTitleStatus> ConfirmMyApprenticeshipTitleStatus()
    {
        //TODO post authentication
        //if (await LatestApprenticeshipIsConfirmed())
        return Task.FromResult(ApprenticePortal.SharedUi.Services.ConfirmMyApprenticeshipTitleStatus.ShowAsConfirmed);
        //return ApprenticePortal.SharedUi.Services.ConfirmMyApprenticeshipTitleStatus.ShowAsRequiringConfirmation;
    }

    public Task<bool> ShowApprenticeFeedback() => Task.FromResult(true);

    public Task<bool> ShowConfirmMyApprenticeship() => Task.FromResult(true);
}
