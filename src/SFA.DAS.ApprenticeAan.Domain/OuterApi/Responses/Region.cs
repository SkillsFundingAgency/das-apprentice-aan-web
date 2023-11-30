using SFA.DAS.Aan.SharedUi.Models;

namespace SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses
{
    public class Region
    {
        public int Id { get; set; }
        public string Area { get; set; } = null!;
        public int Ordering { get; set; }

        public static List<RegionViewModel> RegionToRegionViewModelMapping(List<Region> regions)
        {
            List<RegionViewModel> regionList = new List<RegionViewModel>();
            foreach (Region region in regions.OrderBy(x => x.Ordering))
            {
                regionList.Add(new RegionViewModel { Id = region.Id, Area = region.Area });
            }
            return regionList;
        }
    }
}