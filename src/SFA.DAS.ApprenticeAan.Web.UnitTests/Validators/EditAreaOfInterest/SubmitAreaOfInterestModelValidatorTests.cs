﻿using FluentValidation.TestHelper;
using SFA.DAS.Aan.SharedUi.Models.EditAreaOfInterest;
using SFA.DAS.ApprenticeAan.Web.Validators.EditAreaOfInterest;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.EditAreaOfInterest;

[TestFixture]
public class SubmitAreaOfInterestModelValidatorTests
{
    [Test]
    public void Validate_EventsAndPromotionsAreEmpty_Invalid()
    {
        // Arrange
        var model = new SubmitAreaOfInterestModel
        {
            Events = new List<SelectProfileViewModel>(),
            Promotions = new List<SelectProfileViewModel>()
        };
        var sut = new SubmitAreaOfInterestModelValidator();

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.AreasOfInterest)
            .WithErrorMessage(SubmitAreaOfInterestModelValidator.NoSelectionErrorMessage);
    }

    [Test, MoqAutoData]
    public void Validate_EventsHasValueAndPromotionsAreEmpty_Valid(List<SelectProfileViewModel> selectProfileViewModels)
    {
        // Arrange
        var model = new SubmitAreaOfInterestModel
        {
            Events = selectProfileViewModels,
            Promotions = new List<SelectProfileViewModel>()
        };
        var sut = new SubmitAreaOfInterestModelValidator();

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test, MoqAutoData]
    public void Validate_PromotionsHasValueAndEventsAreEmpty_Valid(List<SelectProfileViewModel> selectProfileViewModels)
    {
        // Arrange
        var model = new SubmitAreaOfInterestModel
        {
            Events = new List<SelectProfileViewModel>(),
            Promotions = selectProfileViewModels
        };
        var sut = new SubmitAreaOfInterestModelValidator();

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test, MoqAutoData]
    public void Validate_PromotionsAndEventsHaveValue_Valid(List<SelectProfileViewModel> selectProfileViewModels)
    {
        // Arrange
        var model = new SubmitAreaOfInterestModel
        {
            Events = selectProfileViewModels,
            Promotions = selectProfileViewModels
        };
        var sut = new SubmitAreaOfInterestModelValidator();

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
