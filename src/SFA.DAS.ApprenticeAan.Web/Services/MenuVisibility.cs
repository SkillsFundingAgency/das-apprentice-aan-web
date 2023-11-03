using SFA.DAS.ApprenticePortal.SharedUi.Services;

namespace SFA.DAS.ApprenticeAan.Web.Services;

public class MenuVisibility : IMenuVisibility
{
    public Task<ConfirmMyApprenticeshipTitleStatus> ConfirmMyApprenticeshipTitleStatus()
    {

        return Task.FromResult(ApprenticePortal.SharedUi.Services.ConfirmMyApprenticeshipTitleStatus.DoNotShow);
    }

    public Task<bool> ShowApprenticeAan() => Task.FromResult(true);

    public Task<bool> ShowApprenticeFeedback() => Task.FromResult(false);

    public Task<bool> ShowConfirmMyApprenticeship() => Task.FromResult(false);
}
