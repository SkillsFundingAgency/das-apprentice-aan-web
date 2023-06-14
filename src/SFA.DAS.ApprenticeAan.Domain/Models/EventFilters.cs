using SFA.DAS.ApprenticeAan.Domain.Constants;

namespace SFA.DAS.ApprenticeAan.Domain.Models;

public class EventFilters
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public List<EventFormat>? EventFormats { get; set; }
}