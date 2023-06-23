using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

namespace SFA.DAS.ApprenticeAan.Web.Interfaces;

public interface IEventSearchQueryStringBuilder
{
    List<SelectedFilter> BuildEventSearchFilters(EventFilterChoices eventFilterChoices, IUrlHelper url);
}