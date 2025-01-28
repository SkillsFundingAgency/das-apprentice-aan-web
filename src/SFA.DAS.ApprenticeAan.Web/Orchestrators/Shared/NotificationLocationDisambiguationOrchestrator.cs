using Microsoft.AspNetCore.Mvc.ModelBinding;
using FluentValidation;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Shared;

namespace SFA.DAS.ApprenticeAan.Web.Orchestrators.Shared
{
    public interface INotificationLocationDisambiguationOrchestrator
    {
        Task<INotificationLocationDisambiguationPartialViewModel> GetViewModel<T>(int radius, string location) where T : INotificationLocationDisambiguationPartialViewModel, new();

        Task<NotificationLocationDisambiguationOrchestrator.RedirectTarget> ApplySubmitModel<T>(
            INotificationLocationDisambiguationPartialSubmitModel submitModel,
            ModelStateDictionary modelState) where T : INotificationLocationsSessionModel, new();
    }

    public class NotificationLocationDisambiguationOrchestrator : INotificationLocationDisambiguationOrchestrator
    {
        private readonly ISessionService _sessionService;
        private readonly IValidator<INotificationLocationDisambiguationPartialSubmitModel> _validator;
        private readonly IOuterApiClient _outerApiClient;

        public NotificationLocationDisambiguationOrchestrator(
            ISessionService sessionService,
            IValidator<INotificationLocationDisambiguationPartialSubmitModel> validator,
            IOuterApiClient apiClient)
        {
            _sessionService = sessionService;
            _validator = validator;
            _outerApiClient = apiClient;
        }

        public async Task<RedirectTarget> ApplySubmitModel<T>(
            INotificationLocationDisambiguationPartialSubmitModel submitModel,
            ModelStateDictionary modelState) where T : INotificationLocationsSessionModel, new()
        {
            var validationResult = await _validator.ValidateAsync(submitModel);
            if (!validationResult.IsValid)
            {
                foreach (var e in validationResult.Errors)
                {
                    modelState.AddModelError(e.PropertyName, e.ErrorMessage);
                }

                return RedirectTarget.Self;
            }

            var sessionModel = _sessionService.Get<T>();

            var apiResponse = await _outerApiClient.GetOnboardingNotificationsLocations(submitModel.SelectedLocation!);

            sessionModel.NotificationLocations.Add(new NotificationLocation
            {
                LocationName = apiResponse.Locations.First().Name,
                GeoPoint = apiResponse.Locations.First().Coordinates,
                Radius = submitModel.Radius
            });

            _sessionService.Set(sessionModel);

            return RedirectTarget.NextPage;
        }

        public async Task<INotificationLocationDisambiguationPartialViewModel> GetViewModel<T>(int radius,
            string location) where T : INotificationLocationDisambiguationPartialViewModel, new()
        {
            var apiResponse = await
                _outerApiClient.GetOnboardingNotificationsLocations(location);

            return new T
            {
                Title = $"We found more than one location that matches '{location}'",
                Radius = radius,
                Location = location,
                Locations = apiResponse.Locations
                    .Select(x => (LocationModel)x)
                    .Take(10)
                    .ToList()
            };
        }

        public enum RedirectTarget
        {
            Self,
            NextPage,
        }
    }
}