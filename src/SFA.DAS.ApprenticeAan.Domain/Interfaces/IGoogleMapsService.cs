namespace SFA.DAS.ApprenticeAan.Domain.Interfaces;

public interface IGoogleMapsService
{
    string GetFullMapUrl(double latitude, double longitude);
    string GetStaticMapUrl(double latitude, double longitude);
}