using FluentAssertions;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Models.Shared;
using SFA.DAS.ApprenticeAan.Web.Validators.Shared;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.Onboarding
{
    [TestFixture]
    public class NotificationsLocationsSubmitModelValidatorTests
    {
        [TestCase("test location", NotificationsLocationsSubmitButtonOption.Add, true, true)]
        [TestCase("test location", NotificationsLocationsSubmitButtonOption.Add, false, true)]
        [TestCase("test location", NotificationsLocationsSubmitButtonOption.Continue, true, true)]
        [TestCase("test location", NotificationsLocationsSubmitButtonOption.Continue, false, true)]
        [TestCase("", NotificationsLocationsSubmitButtonOption.Continue, true, true)]
        [TestCase("", NotificationsLocationsSubmitButtonOption.Continue, false, false)]
        [TestCase("", NotificationsLocationsSubmitButtonOption.Add, true, false)]
        [TestCase("", NotificationsLocationsSubmitButtonOption.Add, true, false)]
        public void Location_Is_Mandatory(string location, string submitOption, bool hasAddedLocations, bool expectIsValid)
        {
            var validator = new NotificationsLocationsSubmitModelValidator();

            var model = new NotificationsLocationsSubmitModel
            {
                HasSubmittedLocations = hasAddedLocations,
                Location = location,
                Radius = 1,
                RadiusOptions = { },
                SubmitButton = submitOption
            };

            var result = validator.Validate(model);

            result.IsValid.Should().Be(expectIsValid);
        }
    }
}
