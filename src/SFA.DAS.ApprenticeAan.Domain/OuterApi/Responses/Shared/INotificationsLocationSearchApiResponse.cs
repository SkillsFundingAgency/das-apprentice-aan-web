namespace SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses.Shared
{
    public class GetNotificationsLocationSearchApiResponse
    {
        public List<Location> Locations { get; set; } = [];

        public class Location
        {
            public string Name { get; set; }
            public double[] Coordinates { get; set; }
        }
    }
}
