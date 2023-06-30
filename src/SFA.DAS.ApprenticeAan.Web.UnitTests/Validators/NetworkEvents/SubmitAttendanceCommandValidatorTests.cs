using FluentValidation.TestHelper;
using SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;
using SFA.DAS.ApprenticeAan.Web.Validators.NetworkEvents;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.NetworkEvents;

[TestFixture]
public class SubmitAttendanceCommandValidatorTests
{
    [Test]
    public void Validate_StartTimeDateInPast_Invalid()
    {
        var model = new SubmitAttendanceCommand
        {
            StartTimeAndDate = DateTime.Now.AddMilliseconds(-5)
        };

        var sut = new SubmitAttendanceCommandValidator();
        var result = sut.TestValidate(model);

        result.ShouldHaveValidationErrorFor(c => c.StartTimeAndDate)
            .WithErrorMessage(SubmitAttendanceCommandValidator.EventTimeDateHasPassed);
    }

    [Test]
    public void Validate_StartTimeDateNow_Valid()
    {
        var model = new SubmitAttendanceCommand
        {
            StartTimeAndDate = DateTime.Now
        };

        var sut = new SubmitAttendanceCommandValidator();
        var result = sut.TestValidate(model);

        result.ShouldHaveAnyValidationError();
    }
}
