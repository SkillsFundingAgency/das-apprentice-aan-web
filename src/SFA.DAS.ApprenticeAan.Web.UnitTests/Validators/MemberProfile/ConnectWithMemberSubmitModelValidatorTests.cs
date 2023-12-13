using FluentValidation.TestHelper;
using SFA.DAS.Aan.SharedUi.Models.PublicProfile;
using SFA.DAS.ApprenticeAan.Web.Validators.MemberProfile;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.MemberProfile;

[TestFixture]
public class ConnectWithMemberSubmitModelValidatorTests
{
    [Test]
    public void Validate_ReasonToGetInTouchIsZero_ReturnInvalid()
    {
        //Arrange
        var model = new ConnectWithMemberSubmitModel
        {
            ReasonToGetInTouch = 0,
            HasAgreedToCodeOfConduct = true,
            HasAgreedToSharePersonalDetails = true
        };

        //Act
        var sut = new MemberProfileSubmitValidator();
        var result = sut.TestValidate(model);

        //Assert
        result.ShouldHaveValidationErrorFor(c => c.ReasonToGetInTouch)
            .WithErrorMessage(MemberProfileSubmitValidator.ReasonToConnectValidationMessage);
    }

    [Test]
    public void Validate_ReasonToGetInTouchIsValid_ReturnValid()
    {
        //Arrange
        var model = new ConnectWithMemberSubmitModel
        {
            ReasonToGetInTouch = 1,
            HasAgreedToCodeOfConduct = true,
            HasAgreedToSharePersonalDetails = true
        };

        //Act
        var sut = new MemberProfileSubmitValidator();
        var result = sut.TestValidate(model);

        //Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ReasonToGetInTouch);
    }

    [Test]
    public void Validate_HasAgreedToCodeOfConductIsFalse_ReturnInvalid()
    {
        //Arrange
        var model = new ConnectWithMemberSubmitModel
        {
            ReasonToGetInTouch = 1,
            HasAgreedToCodeOfConduct = false,
            HasAgreedToSharePersonalDetails = true
        };

        //Act
        var sut = new MemberProfileSubmitValidator();
        var result = sut.TestValidate(model);

        //Assert
        result.ShouldHaveValidationErrorFor(c => c.HasAgreedToCodeOfConduct)
            .WithErrorMessage(MemberProfileSubmitValidator.HasAgreedToCodeOfConductValidationMessage);
    }

    [Test]
    public void Validate_HasAgreedToCodeOfConductIsTrue_ReturnValid()
    {
        //Arrange
        var model = new ConnectWithMemberSubmitModel
        {
            ReasonToGetInTouch = 1,
            HasAgreedToCodeOfConduct = true,
            HasAgreedToSharePersonalDetails = true
        };

        //Act
        var sut = new MemberProfileSubmitValidator();
        var result = sut.TestValidate(model);

        //Assert
        result.ShouldNotHaveValidationErrorFor(x => x.HasAgreedToCodeOfConduct);
    }

    [Test]
    public void Validate_HasAgreedToSharePersonalDetailsIsFalse_ReturnInvalid()
    {
        //Arrange
        var model = new ConnectWithMemberSubmitModel
        {
            ReasonToGetInTouch = 1,
            HasAgreedToCodeOfConduct = true,
            HasAgreedToSharePersonalDetails = false
        };

        //Act
        var sut = new MemberProfileSubmitValidator();
        var result = sut.TestValidate(model);

        //Assert
        result.ShouldHaveValidationErrorFor(c => c.HasAgreedToSharePersonalDetails)
            .WithErrorMessage(MemberProfileSubmitValidator.HasAgreedToSharePersonalDetailsValidationMessage);
    }

    [Test]
    public void Validate_HasAgreedToSharePersonalDetailsIsTrue_ReturnValid()
    {
        //Arrange
        var model = new ConnectWithMemberSubmitModel
        {
            ReasonToGetInTouch = 1,
            HasAgreedToCodeOfConduct = true,
            HasAgreedToSharePersonalDetails = true
        };

        //Act
        var sut = new MemberProfileSubmitValidator();
        var result = sut.TestValidate(model);

        //Assert
        result.ShouldNotHaveValidationErrorFor(x => x.HasAgreedToSharePersonalDetails);
    }
}
