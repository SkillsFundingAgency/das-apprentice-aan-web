using FluentValidation.TestHelper;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.ApprenticeAan.Web.Validators.MemberProfile;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.MemberProfile;

[TestFixture]
public class SubmitConnectionCommandValidatorTests
{
    [Test]
    public void Validate_ReasonToGetInTouchIsZero_ReturnInvalid()
    {
        //Arrange
        var model = new SubmitConnectionCommand
        {
            ReasonToGetInTouch = 0,
            CodeOfConduct = true,
            DetailShareAllowed = true
        };

        //Act
        var sut = new SubmitConnectionCommandValidator();
        var result = sut.TestValidate(model);

        //Assert
        result.ShouldHaveValidationErrorFor(c => c.ReasonToGetInTouch)
            .WithErrorMessage(SubmitConnectionCommandValidator.ReasonToConnectValidationMessage);
    }

    [Test]
    public void Validate_ReasonToGetInTouchIsValid_ReturnValid()
    {
        //Arrange
        var model = new SubmitConnectionCommand
        {
            ReasonToGetInTouch = 1,
            CodeOfConduct = true,
            DetailShareAllowed = true
        };

        //Act
        var sut = new SubmitConnectionCommandValidator();
        var result = sut.TestValidate(model);

        //Assert
        result.ShouldNotHaveValidationErrorFor("ReasonToGetInTouch");
    }

    [Test]
    public void Validate_CodeOfConductIsFalse_ReturnInvalid()
    {
        //Arrange
        var model = new SubmitConnectionCommand
        {
            ReasonToGetInTouch = 1,
            CodeOfConduct = false,
            DetailShareAllowed = true
        };

        //Act
        var sut = new SubmitConnectionCommandValidator();
        var result = sut.TestValidate(model);

        //Assert
        result.ShouldHaveValidationErrorFor(c => c.CodeOfConduct)
            .WithErrorMessage(SubmitConnectionCommandValidator.AcceptCodeOfConductMessage);
    }

    [Test]
    public void Validate_CodeOfConductIsTrue_ReturnValid()
    {
        //Arrange
        var model = new SubmitConnectionCommand
        {
            ReasonToGetInTouch = 1,
            CodeOfConduct = true,
            DetailShareAllowed = true
        };

        //Act
        var sut = new SubmitConnectionCommandValidator();
        var result = sut.TestValidate(model);

        //Assert
        result.ShouldNotHaveValidationErrorFor("CodeOfConduct");
    }

    [Test]
    public void Validate_DetailShareAllowedIsFalse_ReturnInvalid()
    {
        //Arrange
        var model = new SubmitConnectionCommand
        {
            ReasonToGetInTouch = 1,
            CodeOfConduct = true,
            DetailShareAllowed = false
        };

        //Act
        var sut = new SubmitConnectionCommandValidator();
        var result = sut.TestValidate(model);

        //Assert
        result.ShouldHaveValidationErrorFor(c => c.DetailShareAllowed)
            .WithErrorMessage(SubmitConnectionCommandValidator.AcceptDetailShareAllowedMessage);
    }

    [Test]
    public void Validate_DetailShareAllowedIsTrue_ReturnValid()
    {
        //Arrange
        var model = new SubmitConnectionCommand
        {
            ReasonToGetInTouch = 1,
            CodeOfConduct = true,
            DetailShareAllowed = true
        };

        //Act
        var sut = new SubmitConnectionCommandValidator();
        var result = sut.TestValidate(model);

        //Assert
        result.ShouldNotHaveValidationErrorFor("CodeOfConduct");
    }
}
